using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shingrix.UI
{
    public class LowerSparkleView : MonoBehaviour
    {
        [SerializeField]
        private Image sparkleImage;
        Vector3 _rotateVelocity;
        Vector3 _baseScale = Vector3.one;

        void Start()
        {
            _rotateVelocity = new Vector3(0, 0, 1.5f * Time.deltaTime);
        }

        // Update is called once per frame
        void Update()
        {
            sparkleImage.transform.Rotate(_rotateVelocity);
            sparkleImage.transform.localScale = _baseScale + (Mathf.Sin(Time.time) * 0.02f * _baseScale);
        }
    }
}