using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Shingrix.UI
{
    public class GameModeView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_timer;

        [SerializeField]
        private TextMeshProUGUI m_score;

        [SerializeField]
        private TextMeshProUGUI m_name;

        public void SetScoreText(int p_score) {
            m_score.text = p_score.ToString();
        }

        public void SetTimerText(int p_time)
        {
            m_timer.text = p_time.ToString();
        }

        public void SetNameText(string name)
        {
            m_name.text = name;
        }
    }
}