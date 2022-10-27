using Hsinpa.Utility;
using Shingrix.Mode;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Shingrix
{
    public class ShingrixApp : MonoBehaviour
    {
        [Header("Upper Part UI")]
        [SerializeField]
        private GameModeView m_gameModeView;

        [SerializeField]
        private LoginModeView m_loginModeView;

        [SerializeField]
        private VideoPlayer m_idleUpperVideoPlayer;

        [Header("Lower Part UI")]
        [SerializeField]
        private VideoPlayer m_idleLowerVideoPlayer;

        [Header("Mode")]
        [SerializeField]
        private GameModeCtrl m_gameModeCtrl;

        [SerializeField]
        private IdleModeCtrl m_idleModeCtrl;

        [SerializeField]
        private LoginModeCtrl m_loginModeCtrl;

        private IMode m_currentMode;

        private void Awake()
        {
            SimpleEventSystem.Dispose();
            SimpleEventSystem.CustomEventListener += OnSimpleEvent;
        }

        void Start()
        {
            m_idleModeCtrl.SetUp(m_idleUpperVideoPlayer, m_idleLowerVideoPlayer);
            m_gameModeCtrl.SetUp(m_gameModeView);
            m_loginModeCtrl.SetUp(m_loginModeView);

            SetMode(m_idleModeCtrl);
        }

        private void SetMode(IMode mode)
        {
            if (m_currentMode != null)
                m_currentMode.Leave();

            m_currentMode = mode;
            m_currentMode.Enter();
        }

        void OnSimpleEvent(string p_event, params object[] parameters)
        {
            switch (p_event)
            {
                case ShingrixStatic.Event.GameModeTimeup:
                    SetMode(m_idleModeCtrl);
                    break;

                case ShingrixStatic.Event.IdleModeEnter:
                    SetMode(m_idleModeCtrl);
                    break;

                case ShingrixStatic.Event.IdleModeEnd:
                    SetMode(m_loginModeCtrl);
                    break;

                case ShingrixStatic.Event.GameModeEnter:
                    SetMode(m_gameModeCtrl);
                    break;
            }
        }

    }
}