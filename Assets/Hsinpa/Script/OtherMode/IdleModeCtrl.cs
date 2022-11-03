using Hsinpa;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Shingrix.Mode
{
    public class IdleModeCtrl : MonoBehaviour, IMode
    {
        private VideoPlayer m_videoPlayer;
        private CustomActions m_idleInput;

        public void SetUp(VideoPlayer p_videoPlayer)
        {
            m_idleInput = new CustomActions();
            m_videoPlayer = p_videoPlayer;

            m_idleInput.IdleMode.Awake.performed += (callabck) => {
                UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.UI, ShingrixStatic.Audio.EffectTag, ShingrixStatic.Audio.EffectUI);
                Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.LoginModeEnter);
            };
        }
        
        public void Enter()
        {
            m_videoPlayer.gameObject.SetActive(true);

            m_idleInput.IdleMode.Enable();
            m_videoPlayer.time = 0;
            m_videoPlayer.Play();
        }

        public void Leave()
        {
            m_idleInput.IdleMode.Disable();
            m_videoPlayer.Stop();
            m_videoPlayer.gameObject.SetActive(false);
        }
    }
}