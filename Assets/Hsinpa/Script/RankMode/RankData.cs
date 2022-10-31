using FancyScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Data
{
    public class RankData
    {
        private int _score;
        public int Score => _score;

        private int _index;
        public int Index => _index;

        private string _username;
        public string Username => _username;

        private string _id;
        public string ID => _id;

        private bool _isHighlight = false;
        public bool IsHighlight => _isHighlight;

        public RankData(string p_id, int p_score, int p_index, string p_username) {
            this._id = p_id;
            this._score = p_score;
            this._index = p_index;
            this._username = p_username;
        }

        public void SetHighlight() {
            _isHighlight = true;
        }
    }

    public class RankContext : FancyScrollRectContext
    {
        public int SelectedIndex = -1;
        public System.Action<int> OnCellClicked;
    }
}