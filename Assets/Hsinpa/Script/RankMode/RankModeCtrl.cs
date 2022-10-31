using Hsinpa;
using Shingrix.Data;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shingrix.Mode
{
    public class RankModeCtrl : MonoBehaviour, IMode
    {
        LowerSparkleView m_lowerSparkleView;
        RankModeView _rankModelView;
        CustomActions m_rankInput;
        RankModel m_rankModel;


        int _index;

        public void SetUp(RankModeView rankModeView, LowerSparkleView lowerSparkleView, RankModel rankModel) {
            this.m_rankInput = new CustomActions();
            this._rankModelView = rankModeView;
            this.m_lowerSparkleView = lowerSparkleView;
            this.m_rankModel = rankModel;

            m_rankInput.LoginMode.Down.performed += DirectionAction;
            m_rankInput.LoginMode.Up.performed += DirectionAction;
            m_rankInput.LoginMode.Confirm.performed += ConfirmAction;
        }

        public async void Enter()
        {
            _index = 0;
            m_rankInput.LoginMode.Enable();

            var sorted = await m_rankModel.GetScoreSortList();
            _rankModelView.gameObject.SetActive(true);
            _rankModelView.RankScrollView.SetUp(sorted);
            m_lowerSparkleView.gameObject.SetActive(true);
        }

        public void Leave()
        {
            m_rankInput.LoginMode.Disable();
            _rankModelView.gameObject.SetActive(false);
            m_lowerSparkleView.gameObject.SetActive(false);
        }

        public async void LocateToRankStruct(ShingrixStatic.RankStruct rankStruct) {

            await Task.Yield();
            _index = await _rankModelView.RankScrollView.GetIndex(rankStruct);

            if (_index < 0) return;

            _rankModelView.RankScrollView.SetHighLight(_index);
            _rankModelView.RankScrollView.ScrollToIndex(_index, 0.5f);
        }

        private void DirectionAction(InputAction.CallbackContext inputAction)
        {
            int dataCount = m_rankModel.DataArray.Count;
            int step = 4;

            if (inputAction.action == m_rankInput.LoginMode.Down) {
                _index = Mathf.Clamp(_index + step, 0, dataCount -1);
                _rankModelView.RankScrollView.ScrollToIndex(_index, 0.2f);
            }

            if (inputAction.action == m_rankInput.LoginMode.Up)
            {
                _index = Mathf.Clamp(_index - step, 0, dataCount - 1);
                _rankModelView.RankScrollView.ScrollToIndex(_index, 0.8f);
            }
        }

        private void ConfirmAction(InputAction.CallbackContext inputAction)
        {
            Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.LoginModeEnter);
        }
    }
}