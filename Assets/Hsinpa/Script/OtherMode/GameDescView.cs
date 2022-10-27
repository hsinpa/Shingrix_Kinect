using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shingrix.UI
{
    public class GameDescView : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image targetTexture;

        [SerializeField]
        private Sprite[] sprites;

        public System.Action LastTextureReachEvent;

        public void SetTextureIndex(int index) {
            if (sprites == null || index >= sprites.Length) {
                LastTextureReachEvent?.Invoke();
                return;
            }

            targetTexture.sprite = sprites[index];
        }
    }
}