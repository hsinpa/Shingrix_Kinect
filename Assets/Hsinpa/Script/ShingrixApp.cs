using Hsinpa.Utility;
using Shingrix.Data;
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
        private RankModeView m_rankModeView;

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

        [SerializeField]
        private RankModeCtrl m_rankModeCtrl;

        private IMode m_currentMode;
        private RankModel m_rankModel;

        private void Awake()
        {
            SimpleEventSystem.Dispose();
            SimpleEventSystem.CustomEventListener += OnSimpleEvent;
        }

        void Start()
        {
            m_rankModel = new RankModel();
            m_idleModeCtrl.SetUp(m_idleUpperVideoPlayer, m_idleLowerVideoPlayer);
            m_gameModeCtrl.SetUp(m_gameModeView, m_rankModel );
            m_loginModeCtrl.SetUp(m_loginModeView);
            m_rankModeCtrl.SetUp(m_rankModeView, m_rankModel);

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
                    SetMode(m_rankModeCtrl);

                    if (parameters.Length == 1)
                        m_rankModeCtrl.LocateToRankStruct((ShingrixStatic.RankStruct) parameters[0]);
                    break;

                case ShingrixStatic.Event.IdleModeEnter:
                    SetMode(m_idleModeCtrl);
                    break;

                case ShingrixStatic.Event.LoginModeEnter:
                    SetMode(m_loginModeCtrl);
                    break;

                case ShingrixStatic.Event.GameModeEnter:
                    SetMode(m_gameModeCtrl);
                    break;

                case ShingrixStatic.Event.RankModeEnter:
                    SetMode(m_rankModeCtrl);
                    break;
            }
        }

    }
}