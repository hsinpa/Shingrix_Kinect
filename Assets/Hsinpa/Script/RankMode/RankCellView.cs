using FancyScrollView;
using Shingrix.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Shingrix.UI
{
    public class RankCellView : FancyScrollRectCell<RankData, RankContext>
    {
        [SerializeField]
        private TextMeshProUGUI indexField;

        [SerializeField]
        private TextMeshProUGUI usernameField;

        [SerializeField]
        private TextMeshProUGUI scoreField;

        public override void Initialize()
        {

        }

        public override void UpdateContent(RankData itemData)
        {
            indexField.text = (itemData.Index + 1).ToString();
            usernameField.text = itemData.Username;
            scoreField.text = itemData.Score.ToString();
        }
    }
}