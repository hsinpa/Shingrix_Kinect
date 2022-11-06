using Hsinpa;
using Hsinpa.Utility;
using Shingrix.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shingrix.Mode
{
    public class LoginModeCtrl : MonoBehaviour, IMode
    {
        LoginModeView m_loginModeView;
        LowerSparkleView m_lowerSparkleView;
        CustomActions m_loginInput;

        int m_page_index = 0;
        const int m_max_page = 2;

        HintButton m_hintButton;

        Dictionary<HintButton, ActionStructs[]> hintBtnTable = new Dictionary<HintButton, ActionStructs[]>();
        private float m_delay_time;

        public void SetUp(LoginModeView loginModeView, LowerSparkleView lowerSparkleView)
        {
            m_loginInput = new CustomActions();
            m_loginModeView = loginModeView;
            m_lowerSparkleView = lowerSparkleView;

            m_loginInput.LoginMode.NextAction.performed += InputNextAction;
            m_loginInput.LoginMode.Down.performed += DirectionAction;
            m_loginInput.LoginMode.Up.performed += DirectionAction;
            m_loginInput.LoginMode.Right.performed += DirectionAction;
            m_loginInput.LoginMode.Left.performed += DirectionAction;
            m_loginInput.LoginMode.Confirm.performed += ConfirmAction;
            m_loginInput.LoginMode.Any.performed += AnyAction;

            //Name Input Field
            hintBtnTable.Add(m_loginModeView.InputHint, new ActionStructs[] {
                new ActionStructs() {Input = m_loginInput.LoginMode.Confirm, Action = () => ProcessNameField() },
                new ActionStructs() {Input = m_loginInput.LoginMode.Down, Action = () => ProcessNameField() },
                new ActionStructs() {Input = m_loginInput.LoginMode.Left, Action = () => ProcessNameField() },
                new ActionStructs() {Input = m_loginInput.LoginMode.Right, Action = () => ProcessNameField() },
            });

            //Play Input Field
            hintBtnTable.Add(m_loginModeView.PlayHint, new ActionStructs[] {
                new ActionStructs() {Input = m_loginInput.LoginMode.Up, Action = () => SetHintBtn(m_loginModeView.InputHint) },
                new ActionStructs() {Input = m_loginInput.LoginMode.Left, Action = () => SetHintBtn(m_loginModeView.RankHint) },
            });

            //Rank Input Field
            hintBtnTable.Add(m_loginModeView.RankHint, new ActionStructs[] {
                new ActionStructs() {Input = m_loginInput.LoginMode.Up, Action = () => SetHintBtn(m_loginModeView.InputHint) },
                new ActionStructs() {Input = m_loginInput.LoginMode.Right, Action = () => SetHintBtn(m_loginModeView.PlayHint) },
            });
        }

        public void Enter()
        {
            this.m_delay_time = ShingrixStatic.GameMode.LoginBackToIdleTime + Time.time;

            m_loginInput.LoginMode.Enable();
            m_loginModeView.Dispose();
            m_page_index = 0;
            m_loginModeView.gameObject.SetActive(true);
            m_lowerSparkleView.gameObject.SetActive(true);

            m_loginModeView.NameInputField.text = ShingrixStatic.Data.UserName;

            m_loginModeView.RankHint.EnableHintSprite(false);
            m_loginModeView.PlayHint.EnableHintSprite(false);
            SetHintBtn(m_loginModeView.InputHint);
            gameObject.SetActive(true);
        }

        public void Leave()
        {
            m_loginInput.LoginMode.Disable();
            gameObject.SetActive(false);

            m_lowerSparkleView.gameObject.SetActive(false);
            m_loginModeView.gameObject.SetActive(false);
            m_loginModeView.InputHint.SetColor(m_loginModeView.InputHint.OriginalColor);
        }

        private void Update()
        {
            if (!m_loginInput.LoginMode.enabled) return;

            //Overtime, erase data and back to idle page
            if (m_delay_time < Time.time) {
                ShingrixStatic.Data.UserName = "";
                Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.IdleModeEnter);
            }
        }

        #region Input Event
        private void InputNextAction(InputAction.CallbackContext inputAction) {
            if (m_page_index == 0) return;

            m_page_index++;

            if (m_page_index > m_max_page) {
                Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.GameModeEnter);
                return;
            }

            m_loginModeView.EnablePage(m_page_index);
        }

        private void DirectionAction(InputAction.CallbackContext inputAction)
        {
            if (m_page_index > 0) return;

            if (hintBtnTable.TryGetValue(m_hintButton, out var values)) {
                int lens = values.Length;

                for (int i = 0; i < lens; i++) {
                    if (values[i].Input == inputAction.action) {
                        values[i].Action();
                        return;
                    }
                }
            }
        }

        private void ConfirmAction(InputAction.CallbackContext inputAction)
        {
            if (m_page_index > 0) return;

            if (m_hintButton == m_loginModeView.PlayHint) {
                m_page_index++;
                m_loginModeView.EnablePage(m_page_index);
            }

            if (m_hintButton == m_loginModeView.RankHint)
            {
                Debug.Log("Go to Rank Mode");
                Hsinpa.Utility.SimpleEventSystem.Send(ShingrixStatic.Event.RankModeEnter);
            }

            if (m_hintButton == m_loginModeView.InputHint)
            {
                ProcessNameField();
            }
        }

        private void AnyAction(InputAction.CallbackContext inputAction)
        {
            if (m_hintButton == m_loginModeView.InputHint && !m_loginModeView.NameInputField.isFocused) {
                m_loginModeView.NameInputField.ActivateInputField();
                m_loginModeView.NameInputField.Select();
                m_loginModeView.NameInputField.caretPosition = m_loginModeView.NameInputField.text.Length;
            }

            UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.UI, ShingrixStatic.Audio.EffectTag, ShingrixStatic.Audio.EffectUI);
            m_delay_time += ShingrixStatic.GameMode.LoginBackToIdleTime;
        }
        #endregion
        private bool ProcessNameField() {
            bool IsValid = !string.IsNullOrEmpty(m_loginModeView.NameInputField.text);

            if (!IsValid) {
                m_loginModeView.InputHint.SetColor(Color.red);
                m_loginModeView.NameInputField.Select();
                return false;
            }

            ShingrixStatic.Data.UserName = m_loginModeView.NameInputField.text;
            m_loginModeView.InputHint.SetColor(m_loginModeView.InputHint.OriginalColor);
            SetHintBtn(m_loginModeView.PlayHint);
            return true;
        }

        private void SetHintBtn(HintButton hintButton) {
            if (m_hintButton != null)
                m_hintButton.EnableHintSprite(false);

            m_hintButton = hintButton;
            m_hintButton.EnableHintSprite(true);

            if (m_hintButton == m_loginModeView.InputHint) {
                m_loginModeView.NameInputField.ActivateInputField();
                m_loginModeView.NameInputField.Select();
                //m_loginModeView.NameInputField.caretPosition = m_loginModeView.NameInputField.text.Length;
            }
            else
                m_loginModeView.NameInputField.DeactivateInputField(clearSelection: true);

        }

        public struct ActionStructs {
            public InputAction Input;
            public System.Action Action;
        }
    }
}