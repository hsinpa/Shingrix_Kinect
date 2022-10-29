using FancyScrollView;
using Shingrix.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shingrix.UI
{
    public class RankScrollView : FancyScrollRect<RankData, RankContext>
    {
        [SerializeField] GameObject cellPrefab = default;
        private float _cellSize= 15;

        protected override GameObject CellPrefab => cellPrefab;

        protected override float CellSize => _cellSize;

        public void SetUp(List<ShingrixStatic.RankStruct> rankStructs) {
            List<RankData> rankList = new List<RankData>();
            int rankStructLen = rankStructs.Count;

            for (int i = 0; i < rankStructLen; i++) {
                rankList.Add( new RankData(rankStructs[i].score, i, rankStructs[i].name) );
            }

            UpdateContents(rankList);
        }

        public void ScrollToIndex(int p_index, float p_alignment) {
            ScrollTo(p_index, 0.1f, alignment: p_alignment);
        } 

        public void Dispose()
        {

        }
    }
}