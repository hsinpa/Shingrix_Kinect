using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Shingrix.UI
{
    public class GameModeView : MonoBehaviour
    {

        [Header("Static")]
        [SerializeField]
        private TextMeshProUGUI m_timer;

        [SerializeField]
        private TextMeshProUGUI m_score;

        [SerializeField]
        private TextMeshProUGUI m_name;

        [SerializeField]
        private TextMeshProUGUI m_life_save;

        [Header("Page")]
        [SerializeField]
        private CanvasGroup readyCanvas;

        [SerializeField]
        private GameObject staticCanvas;

        [SerializeField]
        private GameObject endingCanvas;

        public void SetScoreText(int p_score) {
            m_score.text = p_score.ToString();
            m_life_save.text = p_score.ToString();
        }

        public void SetTimerText(int p_time)
        {
            m_timer.text = p_time.ToString();
        }

        public void SetNameText(string name)
        {
            m_name.text = name;
        }

        public void ShowStaticBoard() {
            readyCanvas.gameObject.SetActive(false);
            staticCanvas.gameObject.SetActive(true);
            endingCanvas.gameObject.SetActive(false);
        }

        public void ShowReadyUI()
        {
            readyCanvas.gameObject.SetActive(true);
            endingCanvas.gameObject.SetActive(false);
            staticCanvas.gameObject.SetActive(false);
        }

        public void ShowEndUI()
        {
            readyCanvas.gameObject.SetActive(false);
            endingCanvas.gameObject.SetActive(true);
            staticCanvas.gameObject.SetActive(false);
        }
    }
}