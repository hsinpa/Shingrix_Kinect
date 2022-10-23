using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Render {
    public class CameraOutputTexture : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private Camera uiCamera;

        [SerializeField]
        private RenderTexture renderTextureInput;

        void Start()
        {
            mainCamera.targetTexture = renderTextureInput;
            uiCamera.targetTexture = mainCamera.targetTexture;
        }

        void Update()
        {
            uiCamera.Render(); // not sure why this is necessary but the first render will not render the UI
            mainCamera.Render(); // Will render both the base and overlay cameras
        }
    }
}
