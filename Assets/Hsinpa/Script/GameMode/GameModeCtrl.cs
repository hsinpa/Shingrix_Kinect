using Hsinpa.KinectWrap;
using Hsinpa.Utility;
using Shingrix.Mode.Game;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode
{
    public class GameModeCtrl : MonoBehaviour, IMode
    {
        [SerializeField]
        private BacteriaObject m_bacteriaPrefab;

        [SerializeField]
        private CustomBodyView m_customBodyView;

        private GameModeView m_gameModeView;
        private BacteriaSpawner m_bacteriaSpawner;
        private DigitalTimer m_digitalTimer;

        private ITracker m_kinectTracker;
        private ITracker m_mouseTracker;

        private CutterHandler m_cutter;
        
        private int m_score_point;

        public void SetUp(GameModeView gameModeView) {
            m_gameModeView = gameModeView;
            m_digitalTimer = new DigitalTimer();
            m_digitalTimer.SetTimeType(DigitalTimer.Type.Timer_CountDown);
            m_bacteriaSpawner = new BacteriaSpawner(m_bacteriaPrefab, this.transform);

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

            _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(ShingrixStatic.GameMode.WaitReadyTime, () =>
            {
                m_gameModeView.ShowStaticBoard();
                m_digitalTimer.StartTimer(ShingrixStatic.GameMode.Time);
            });
        }

        public void Leave()
        {
            ShingrixStatic.Data.UserScore = m_score_point;
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

            _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(ShingrixStatic.GameMode.WaitEndingTime, 
                () => {
                    if (this != null)
                        SimpleEventSystem.Send(ShingrixStatic.Event.GameModeTimeup, m_score_point);
                });
        }

        private void OnBacteriaCutEvent() {
                m_score_point++;
                m_gameModeView?.SetScoreText(m_score_point);
        }
    }
}