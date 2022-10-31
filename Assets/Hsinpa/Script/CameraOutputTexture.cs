using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Hsinpa.Render {
    public class CameraOutputTexture : MonoBehaviour
    {

        #region Inspector Parameters
        [Header("SetUp")]
        [SerializeField]
        private RenderTexture renderTextureInput;

        [SerializeField]
        private RenderTexture renderTextureOutput;

        [SerializeField]
        private RawImage renderRawImage;

        [SerializeField]
        private RawImage debugRawImage;

        [SerializeField]
        private Material repositionMaterial;

        [Header("Config")]
        [SerializeField]
        private bool IsDebugMode;

        [SerializeField]
        private Vector2Int UpperTextureSize = new Vector2Int(384, 640);

        [SerializeField]
        private Vector2Int LowerTextureSize = new Vector2Int(384, 512);

        [SerializeField]
        private Vector2Int UpperTextureTargetSize = new Vector2Int(384, 640);

        [SerializeField]
        private Vector2Int LowerTextureTargetSize = new Vector2Int(252, 336);

        [SerializeField]
        private Vector2Int UpperTextureTargetPosition = new Vector2Int(0 ,0);

        [SerializeField]
        private Vector2Int LowerTextureTargetPosition = new Vector2Int(500, 0);
        #endregion

        Hsinpa.CustomActions customInput;

        void Start()
        {
            customInput = new CustomActions();
            customInput.GameMode.Enable();

            debugRawImage.gameObject.SetActive(false);
            renderRawImage.texture = renderTextureOutput;
            debugRawImage.texture = renderTextureOutput;

            if (IsDebugMode) {
                debugRawImage.gameObject.SetActive(true);
                renderRawImage.texture = renderTextureInput;
            }

            //customInput.GameMode.CaptureScreen.performed += CaptureScreen_performed;
        }

        void Update()
        {
            RepositionRendering();
        }

        private void RepositionRendering() {
            Graphics.Blit(renderTextureInput, renderTextureOutput, repositionMaterial);
        }

        private void CaptureScreen_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "screenshot_tex.png");
            Utility.UtilityFunc.SaveRTToFile(renderTextureOutput, path);
        }

        public struct RepositionConfig {
            public Vector2 textureAPosition;
            public Vector2 textureASize;

            public Vector2 textureBPosition;
            public Vector2 textureBSize;

            public Vector2 textureAPositionOffset;
            public Vector2 textureASizeOffset;
        }

    }
}
