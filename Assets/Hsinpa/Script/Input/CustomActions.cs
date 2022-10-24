//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Hsinpa/Input/CustomActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Hsinpa
{
    public partial class @CustomActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @CustomActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""CustomActions"",
    ""maps"": [
        {
            ""name"": ""InputAction"",
            ""id"": ""d4a94d3a-130e-48db-a1e4-05aae248d0e3"",
            ""actions"": [
                {
                    ""name"": ""CaptureScreen"",
                    ""type"": ""Button"",
                    ""id"": ""9d0781b1-15a8-4098-b4cf-c1a9ceadebc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""896cec95-623d-40d5-8af2-b46433dc5e2c"",
                    ""path"": ""<Keyboard>/#(P)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CaptureScreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // InputAction
            m_InputAction = asset.FindActionMap("InputAction", throwIfNotFound: true);
            m_InputAction_CaptureScreen = m_InputAction.FindAction("CaptureScreen", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // InputAction
        private readonly InputActionMap m_InputAction;
        private IInputActionActions m_InputActionActionsCallbackInterface;
        private readonly InputAction m_InputAction_CaptureScreen;
        public struct InputActionActions
        {
            private @CustomActions m_Wrapper;
            public InputActionActions(@CustomActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @CaptureScreen => m_Wrapper.m_InputAction_CaptureScreen;
            public InputActionMap Get() { return m_Wrapper.m_InputAction; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(InputActionActions set) { return set.Get(); }
            public void SetCallbacks(IInputActionActions instance)
            {
                if (m_Wrapper.m_InputActionActionsCallbackInterface != null)
                {
                    @CaptureScreen.started -= m_Wrapper.m_InputActionActionsCallbackInterface.OnCaptureScreen;
                    @CaptureScreen.performed -= m_Wrapper.m_InputActionActionsCallbackInterface.OnCaptureScreen;
                    @CaptureScreen.canceled -= m_Wrapper.m_InputActionActionsCallbackInterface.OnCaptureScreen;
                }
                m_Wrapper.m_InputActionActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @CaptureScreen.started += instance.OnCaptureScreen;
                    @CaptureScreen.performed += instance.OnCaptureScreen;
                    @CaptureScreen.canceled += instance.OnCaptureScreen;
                }
            }
        }
        public InputActionActions @InputAction => new InputActionActions(this);
        public interface IInputActionActions
        {
            void OnCaptureScreen(InputAction.CallbackContext context);
        }
    }
}