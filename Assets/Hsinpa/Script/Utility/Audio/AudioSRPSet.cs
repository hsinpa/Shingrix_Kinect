using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Utility
{
    public class AudioSRPSet : MonoBehaviour
    {
        [SerializeField]
        private List<AudioSRP> audioSet;

        public AudioClip GetAudioClip(string tag, string id) {

            if (audioSet == null) return null;

            var audioTag =  audioSet.Find(x => x.tag == tag);

            return audioTag.audioSets.Find(x => id == x.id).audioClip;
        }
    }
}