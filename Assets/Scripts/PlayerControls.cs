// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""af7b9a6e-29c4-471e-9567-7215431f3cf5"",
            ""actions"": [
                {
                    ""name"": ""PrimaryTouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6de5bdb8-e89a-44bd-a1ee-d29e0cd4521f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""PrimaryTouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2dfaeec1-0529-4d7a-9f64-8a7241f63917"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Press"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2ceef6d0-e15d-498a-a5ce-526d4dbecb45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SecondaryTouch"",
                    ""type"": ""Button"",
                    ""id"": ""7c756535-a4de-4cfb-be2f-2ad88066b998"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26446bd9-d1df-419d-afda-effee9645983"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ed3cd7a-08fe-48aa-9992-6aadfb0ffdaa"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0507f3ea-85a1-4c30-a5a6-0a4031059b84"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66e26c5c-16d5-4bb3-a8c4-ca3e9cfa04d7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a16a3bb-06fb-4b17-bb7e-2bbc3753c0b6"",
                    ""path"": ""<Touchscreen>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c6947b8-7a6b-4b70-8e75-035f35fcfb7e"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05d8400c-2878-45f1-9d5c-cf5bd042aca9"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_PrimaryTouch = m_Controls.FindAction("PrimaryTouch", throwIfNotFound: true);
        m_Controls_PrimaryTouchPosition = m_Controls.FindAction("PrimaryTouchPosition", throwIfNotFound: true);
        m_Controls_Press = m_Controls.FindAction("Press", throwIfNotFound: true);
        m_Controls_SecondaryTouch = m_Controls.FindAction("SecondaryTouch", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_PrimaryTouch;
    private readonly InputAction m_Controls_PrimaryTouchPosition;
    private readonly InputAction m_Controls_Press;
    private readonly InputAction m_Controls_SecondaryTouch;
    public struct ControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryTouch => m_Wrapper.m_Controls_PrimaryTouch;
        public InputAction @PrimaryTouchPosition => m_Wrapper.m_Controls_PrimaryTouchPosition;
        public InputAction @Press => m_Wrapper.m_Controls_Press;
        public InputAction @SecondaryTouch => m_Wrapper.m_Controls_SecondaryTouch;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @PrimaryTouch.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryTouch.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryTouch.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryTouchPosition.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPrimaryTouchPosition;
                @Press.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPress;
                @Press.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPress;
                @Press.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPress;
                @SecondaryTouch.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondaryTouch;
                @SecondaryTouch.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondaryTouch;
                @SecondaryTouch.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondaryTouch;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryTouch.started += instance.OnPrimaryTouch;
                @PrimaryTouch.performed += instance.OnPrimaryTouch;
                @PrimaryTouch.canceled += instance.OnPrimaryTouch;
                @PrimaryTouchPosition.started += instance.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.performed += instance.OnPrimaryTouchPosition;
                @PrimaryTouchPosition.canceled += instance.OnPrimaryTouchPosition;
                @Press.started += instance.OnPress;
                @Press.performed += instance.OnPress;
                @Press.canceled += instance.OnPress;
                @SecondaryTouch.started += instance.OnSecondaryTouch;
                @SecondaryTouch.performed += instance.OnSecondaryTouch;
                @SecondaryTouch.canceled += instance.OnSecondaryTouch;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);
    public interface IControlsActions
    {
        void OnPrimaryTouch(InputAction.CallbackContext context);
        void OnPrimaryTouchPosition(InputAction.CallbackContext context);
        void OnPress(InputAction.CallbackContext context);
        void OnSecondaryTouch(InputAction.CallbackContext context);
    }
}
