using Hsinpa.KinectWrap;
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
        private ITracker m_kinectTracker;
        private ITracker m_mouseTracker;

        private CutterHandler m_cutter;

        public void SetUp(GameModeView gameModeView) {
            m_gameModeView = gameModeView;
            m_bacteriaSpawner = new BacteriaSpawner(m_bacteriaPrefab, this.transform);

            m_kinectTracker = new KinectTracker(m_customBodyView);
            m_mouseTracker = new MouseTracker(Camera.main);

            m_cutter = new CutterHandler(m_bacteriaSpawner, m_kinectTracker);
        }

        public void Enter()
        {
            gameObject.SetActive(true);
            m_gameModeView?.SetScoreText(0);
            m_gameModeView?.gameObject.SetActive(true);
        }

        public void Leave()
        {
            gameObject.SetActive(false);
            m_gameModeView?.gameObject.SetActive(false);
        }

        private void Update()
        {
            //m_mouseTracker.OnUpdate();
            m_kinectTracker.OnUpdate();
            m_cutter.OnUpdate();
            m_bacteriaSpawner.OnUpdate();
        }
    }
}