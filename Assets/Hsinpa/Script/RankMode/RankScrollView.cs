using FancyScrollView;
using Shingrix.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Shingrix.UI
{
    public class RankScrollView : FancyScrollRect<RankData, RankContext>
    {
        [SerializeField] GameObject cellPrefab = default;
        private float _cellSize= 15;

        protected override GameObject CellPrefab => cellPrefab;

        protected override float CellSize => _cellSize;
        List<RankData> m_rankList = new List<RankData>();

        public void SetUp(List<ShingrixStatic.RankStruct> rankStructs) {
            this.m_rankList.Clear();
            int rankStructLen = rankStructs.Count;

            for (int i = 0; i < rankStructLen; i++) {
                this.m_rankList.Add( new RankData(rankStructs[i].id, rankStructs[i].score, i, rankStructs[i].name) );
            }

            UpdateContents(this.m_rankList);
        }

        public async Task<int> GetIndex(ShingrixStatic.RankStruct rankStruct)
        {
            return await Task.Run(() =>
            {
                return m_rankList.FindIndex(x => x.ID == rankStruct.id);
            });
        }

        public void SetHighLight(int p_index) {
            m_rankList[p_index].SetHighlight();
            UpdateContents(this.m_rankList);
        }

        public async void ScrollToIndex(int p_index, float p_alignment) {
            await Task.Yield();

            ScrollTo(p_index, 0.1f, alignment: p_alignment);        
        } 

        public void Dispose()
        {

        }
    }
}