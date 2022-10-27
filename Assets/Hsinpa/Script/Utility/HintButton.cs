using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.UI
{
    public class HintButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image hintSprite;

        private Color _originalColor;
        public Color OriginalColor => _originalColor;

        private void Start()
        {
            _originalColor = hintSprite.color;
        }

        public void EnableHintSprite(bool enable)
        {
            hintSprite.enabled = enable;
        }

        public void SetColor(Color color) {
            hintSprite.color = color;
        }
    }
}