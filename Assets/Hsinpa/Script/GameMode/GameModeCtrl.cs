using Hsinpa.KinectWrap;
using Hsinpa.Utility;
using Shingrix.Data;
using Shingrix.Mode.Game;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Shingrix.Mode
{
    public class GameModeCtrl : MonoBehaviour, IMode
    {
        [SerializeField]
        private BacteriaObject m_bacteriaPrefab;

        [SerializeField]
        private BacteriaObject m_superPrefab;

        [SerializeField]
        private ParticleSystem m_breakParticlePrefab;

        [SerializeField]
        private VisualEffect m_slashParticlePrefab;

        [SerializeField]
        private CustomBodyView m_customBodyView;

        private GameModeView m_gameModeView;
        private BacteriaSpawner m_bacteriaSpawner;
        private DigitalTimer m_digitalTimer;

        private ITracker m_kinectTracker;
        private ITracker m_mouseTracker;

        private CutterHandler m_cutter;
        
        private int m_score_point;
        private RankModel m_rankModel;
        ShingrixStatic.RankStruct m_rankStruct;

        public void SetUp(GameModeView gameModeView, RankModel rankModel) {
            m_gameModeView = gameModeView;
            m_rankModel = rankModel;
            m_digitalTimer = new DigitalTimer();
            m_digitalTimer.SetTimeType(DigitalTimer.Type.Timer_CountDown);
            m_bacteriaSpawner = new BacteriaSpawner(m_bacteriaPrefab, m_superPrefab, m_breakParticlePrefab, m_slashParticlePrefab, this.transform);

            m_kinectTracker = new KinectTracker(m_customBodyView);
            m_mouseTracker = new MouseTracker(Camera.main);

            m_cutter = new CutterHandler(m_bacteriaSpawner, m_mouseTracker);
            m_cutter.BacteriaCutEvent += OnBacteriaCutEvent;
        }

        public void Enter()
        {
            m_score_point = 0;
            m_digitalTimer.ResetTimer();

            m_gameModeView?.SetNameText(ShingrixStatic.Data.UserName);
            m_gameModeView?.SetScoreText(m_score_point);
            m_gameModeView?.gameObject.SetActive(true);
            m_gameModeView.ShowReadyUI();
            gameObject.SetActive(true);

            m_rankStruct = new ShingrixStatic.RankStruct { id = System.Guid.NewGuid().ToString(), name = ShingrixStatic.Data.UserName};

            _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(ShingrixStatic.GameMode.WaitReadyTime, () =>
            {
                m_gameModeView.ShowStaticBoard();
                m_digitalTimer.StartTimer(ShingrixStatic.GameMode.Time);
            });
        }

        public void Leave()
        {
            ShingrixStatic.Data.UserName = "";
            m_rankStruct.score = m_score_point;
            m_rankStruct.timestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
            m_rankModel.PushCurrentRankStruct(m_rankStruct);
            m_rankModel.SaveToDisk();

            gameObject.SetActive(false);
            m_gameModeView?.gameObject.SetActive(false);
            m_digitalTimer.StopTimer();
            m_gameModeView.ShowReadyUI();
        }

        private void Update()
        {
            if (!m_digitalTimer.TimerState) return;

            m_mouseTracker.OnUpdate();
            m_kinectTracker.OnUpdate();
            m_cutter.OnUpdate();
            m_bacteriaSpawner.OnUpdate();

            int time_leave = m_digitalTimer.GetSecond();
            m_gameModeView.SetTimerText(time_leave);

            if (m_digitalTimer.GetSecond() <= 0) {
                m_digitalTimer.StopTimer();
                ExecTimeUpAction();
            }
        }

        private void ExecTimeUpAction() {
            m_bacteriaSpawner.Dispose();
            m_cutter.Dispose();
            m_gameModeView.ShowEndUI();

            UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.UI, ShingrixStatic.Audio.EffectTag, ShingrixStatic.Audio.EffectEnd);

            _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(ShingrixStatic.GameMode.WaitEndingTime, 
                () => {
                    if (this != null) {
                        SimpleEventSystem.Send(ShingrixStatic.Event.GameModeTimeup, m_rankStruct);
                    }
                });
        }

        private void OnBacteriaCutEvent(BacteriaObject.Type type, Vector3 cutPosition, Vector3 cutScale, Vector3 normal) {
            m_score_point += (type == BacteriaObject.Type.Bateria) ? 1 : -1;
            m_gameModeView?.SetScoreText(m_score_point);
            var breakParicleGamobject = Pooling.PoolManager.instance?.ReuseObject(ShingrixStatic.Event.ObjPoolKeybreakParticle);
            var slashParicleGamobject = Pooling.PoolManager.instance?.ReuseObject(ShingrixStatic.Event.ObjPoolKeySlashParticle);

            try
            {
                var breakParticle = breakParicleGamobject.GetComponent<ParticleSystem>();
                breakParticle.transform.position = cutPosition;
                breakParticle.transform.localScale = cutScale;
                breakParticle.Play();

                var slashParticle = slashParicleGamobject.GetComponent<VisualEffect>();

                slashParicleGamobject.transform.position = cutPosition;
                normal.z = 0;
                slashParicleGamobject.transform.rotation = Quaternion.FromToRotation(slashParicleGamobject.transform.right, normal) * slashParicleGamobject.transform.rotation;
                slashParticle.enabled = true;
                slashParticle.playRate = 3;
                slashParticle.Play();

                _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(1, () => {
                    slashParticle.enabled = false;

                    Pooling.PoolManager.instance?.Destroy(breakParicleGamobject);
                    Pooling.PoolManager.instance?.Destroy(slashParicleGamobject);
                });
            }
            catch {
                Pooling.PoolManager.instance?.Destroy(breakParicleGamobject);
            }

            PlayBateriaCutSound(type == BacteriaObject.Type.Bateria);
        }

        private void PlayBateriaCutSound(bool positiveSound) {
            int randomIndex = Random.Range(1, 4);
            string success_id = string.Format(ShingrixStatic.Audio.EffectHit, randomIndex);
            string audio_id = (positiveSound) ? success_id : ShingrixStatic.Audio.EffectHitWrong;
            UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.UI, ShingrixStatic.Audio.EffectTag, audio_id);
        }
    }
}