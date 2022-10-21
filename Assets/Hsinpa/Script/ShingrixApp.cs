using Shingrix.Mode;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix
{
    public class ShingrixApp : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private GameModeView m_gameModeView;

        [Header("Mode")]
        [SerializeField]
        private GameModeCtrl m_gameModeCtrl;

        void Start()
        {
            m_gameModeCtrl.SetUp(m_gameModeView);
            m_gameModeCtrl.Enter();
        }
    }
}