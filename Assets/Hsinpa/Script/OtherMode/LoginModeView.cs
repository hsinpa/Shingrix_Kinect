using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shingrix.UI
{
    public class LoginModeView : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField nameInputField;
        public TMPro.TMP_InputField NameInputField => nameInputField;

        [SerializeField]
        private HintButton inputHint;
        public HintButton InputHint => inputHint;

        [SerializeField]
        private HintButton rankHint;
        public HintButton RankHint => rankHint;

        [SerializeField]
        private HintButton playHint;
        public HintButton PlayHint => playHint;

        [SerializeField]
        private GameObject[] pages;
        public GameObject[] Pages => pages;

        public void Dispose() {
            nameInputField.text = "";
            EnablePage(0);
        }

        public void EnablePage(int page_index) {
            int pageLens = pages.Length;

            for (int i = 0; i < pageLens; i++) {
                if (i == page_index) {
                    pages[i].gameObject.SetActive(true);
                    continue;
                }

                pages[i].gameObject.SetActive(false);
            }
        }

    }
}