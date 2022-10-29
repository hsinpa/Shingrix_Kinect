using Hsinpa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Shingrix.Mode
{
    public class IdleModeCtrl : MonoBehaviour, IMode
    {
        private VideoPlayer m_upperVideoPlayer;
        private VideoPlayer m_lowerVideoPlayer;
        private CustomActions m_idleInput;

        public void SetUp(VideoPlayer upperVideoPlayer, VideoPlayer lowerVideoPlayer)
        {
            m_idleInput = new CustomActions();
            m_upperVideoPlayer = upperVideoPlayer;
            m_lowerVideoPlayer = lowerVideoPlayer;

            m_idleInput.IdleMode.Awake.performed += (callabck) => {
                Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.LoginModeEnter);
            };
        }
        
        public void Enter()
        {
            m_upperVideoPlayer.gameObject.SetActive(true);
            m_lowerVideoPlayer.gameObject.SetActive(true);

            m_idleInput.IdleMode.Enable();
            m_upperVideoPlayer.time = 0;
            m_lowerVideoPlayer.time = 0;
            m_upperVideoPlayer.Play();
            m_lowerVideoPlayer.Play();
        }

        public void Leave()
        {
            m_idleInput.IdleMode.Disable();
            m_upperVideoPlayer.Stop();
            m_lowerVideoPlayer.Stop();
            m_upperVideoPlayer.gameObject.SetActive(false);
            m_lowerVideoPlayer.gameObject.SetActive(false);
        }
    }
}