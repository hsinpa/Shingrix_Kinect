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
        private GameObject m_bacteriaPrefab;

        private GameModeView m_gameModeView;
        private BacteriaSpawner m_bacteriaSpawner;

        public void SetUp(GameModeView gameModeView) {
            m_gameModeView = gameModeView;
            m_bacteriaSpawner = new BacteriaSpawner(m_bacteriaPrefab, this.transform);
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
            m_bacteriaSpawner.OnUpdate();
        }
    }
}