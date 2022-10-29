using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.UI
{
    public class RankModeView : MonoBehaviour
    {
        [SerializeField]
        private RankScrollView rankScrollView;
        public RankScrollView RankScrollView => rankScrollView;
    }
}