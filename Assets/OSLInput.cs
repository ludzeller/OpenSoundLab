//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/OSLInput.inputactions
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

public partial class @OSLInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @OSLInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""OSLInput"",
    ""maps"": [
        {
            ""name"": ""Patcher"",
            ""id"": ""fa0e3d9e-1417-446a-b28e-db7409fca285"",
            ""actions"": [
                {
                    ""name"": ""PrimaryLeft"",
                    ""type"": ""Button"",
                    ""id"": ""e6c9ecaf-b2ef-493b-b7eb-3305dc359157"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrimaryRight"",
                    ""type"": ""Button"",
                    ""id"": ""b1dd8378-1cf0-4c93-a1ed-4a65ecfe40e9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryLeft"",
                    ""type"": ""Button"",
                    ""id"": ""cfe2a8e1-0def-4b73-9835-7999d7775914"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryRight"",
                    ""type"": ""Button"",
                    ""id"": ""0284a21b-ad71-4fdf-ab6c-d2cf9970840a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GripLeft"",
                    ""type"": ""Button"",
                    ""id"": ""8c80dbb2-e4df-4247-a62f-a9443cb1a57a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GripRight"",
                    ""type"": ""Button"",
                    ""id"": ""95bc30e0-54ac-4b4e-97e9-af5ac3958eef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TriggerLeft"",
                    ""type"": ""Button"",
                    ""id"": ""0b5b3bc8-f9ea-460f-8591-9c0e32cce049"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TriggerRight"",
                    ""type"": ""Button"",
                    ""id"": ""59544473-b7c3-4794-a054-d2f00baf676c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""be1bdf47-24c3-4128-be97-61de38913547"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3452e574-b40a-4b90-acf7-27cf147755c9"",
                    ""path"": ""<XRController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c0698d6-a459-49d2-809d-51d63aaa94ce"",
                    ""path"": ""<XRController>{LeftHand}/gripButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d135410e-b60a-4c99-bf76-1522864a3612"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80ca9fb8-b10b-4b0b-8563-ebb23963f059"",
                    ""path"": ""<XRController>{RightHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5754cf6e-9933-4238-9d51-1aab7c3ea138"",
                    ""path"": ""<XRController>{RightHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f96c9781-0c26-43d0-9bef-b7c8934d0615"",
                    ""path"": ""<XRController>{RightHand}/gripButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GripRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb73339d-971e-4352-b16a-64660a902881"",
                    ""path"": ""<XRController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Patcher
        m_Patcher = asset.FindActionMap("Patcher", throwIfNotFound: true);
        m_Patcher_PrimaryLeft = m_Patcher.FindAction("PrimaryLeft", throwIfNotFound: true);
        m_Patcher_PrimaryRight = m_Patcher.FindAction("PrimaryRight", throwIfNotFound: true);
        m_Patcher_SecondaryLeft = m_Patcher.FindAction("SecondaryLeft", throwIfNotFound: true);
        m_Patcher_SecondaryRight = m_Patcher.FindAction("SecondaryRight", throwIfNotFound: true);
        m_Patcher_GripLeft = m_Patcher.FindAction("GripLeft", throwIfNotFound: true);
        m_Patcher_GripRight = m_Patcher.FindAction("GripRight", throwIfNotFound: true);
        m_Patcher_TriggerLeft = m_Patcher.FindAction("TriggerLeft", throwIfNotFound: true);
        m_Patcher_TriggerRight = m_Patcher.FindAction("TriggerRight", throwIfNotFound: true);
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

    // Patcher
    private readonly InputActionMap m_Patcher;
    private List<IPatcherActions> m_PatcherActionsCallbackInterfaces = new List<IPatcherActions>();
    private readonly InputAction m_Patcher_PrimaryLeft;
    private readonly InputAction m_Patcher_PrimaryRight;
    private readonly InputAction m_Patcher_SecondaryLeft;
    private readonly InputAction m_Patcher_SecondaryRight;
    private readonly InputAction m_Patcher_GripLeft;
    private readonly InputAction m_Patcher_GripRight;
    private readonly InputAction m_Patcher_TriggerLeft;
    private readonly InputAction m_Patcher_TriggerRight;
    public struct PatcherActions
    {
        private @OSLInput m_Wrapper;
        public PatcherActions(@OSLInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryLeft => m_Wrapper.m_Patcher_PrimaryLeft;
        public InputAction @PrimaryRight => m_Wrapper.m_Patcher_PrimaryRight;
        public InputAction @SecondaryLeft => m_Wrapper.m_Patcher_SecondaryLeft;
        public InputAction @SecondaryRight => m_Wrapper.m_Patcher_SecondaryRight;
        public InputAction @GripLeft => m_Wrapper.m_Patcher_GripLeft;
        public InputAction @GripRight => m_Wrapper.m_Patcher_GripRight;
        public InputAction @TriggerLeft => m_Wrapper.m_Patcher_TriggerLeft;
        public InputAction @TriggerRight => m_Wrapper.m_Patcher_TriggerRight;
        public InputActionMap Get() { return m_Wrapper.m_Patcher; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PatcherActions set) { return set.Get(); }
        public void AddCallbacks(IPatcherActions instance)
        {
            if (instance == null || m_Wrapper.m_PatcherActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PatcherActionsCallbackInterfaces.Add(instance);
            @PrimaryLeft.started += instance.OnPrimaryLeft;
            @PrimaryLeft.performed += instance.OnPrimaryLeft;
            @PrimaryLeft.canceled += instance.OnPrimaryLeft;
            @PrimaryRight.started += instance.OnPrimaryRight;
            @PrimaryRight.performed += instance.OnPrimaryRight;
            @PrimaryRight.canceled += instance.OnPrimaryRight;
            @SecondaryLeft.started += instance.OnSecondaryLeft;
            @SecondaryLeft.performed += instance.OnSecondaryLeft;
            @SecondaryLeft.canceled += instance.OnSecondaryLeft;
            @SecondaryRight.started += instance.OnSecondaryRight;
            @SecondaryRight.performed += instance.OnSecondaryRight;
            @SecondaryRight.canceled += instance.OnSecondaryRight;
            @GripLeft.started += instance.OnGripLeft;
            @GripLeft.performed += instance.OnGripLeft;
            @GripLeft.canceled += instance.OnGripLeft;
            @GripRight.started += instance.OnGripRight;
            @GripRight.performed += instance.OnGripRight;
            @GripRight.canceled += instance.OnGripRight;
            @TriggerLeft.started += instance.OnTriggerLeft;
            @TriggerLeft.performed += instance.OnTriggerLeft;
            @TriggerLeft.canceled += instance.OnTriggerLeft;
            @TriggerRight.started += instance.OnTriggerRight;
            @TriggerRight.performed += instance.OnTriggerRight;
            @TriggerRight.canceled += instance.OnTriggerRight;
        }

        private void UnregisterCallbacks(IPatcherActions instance)
        {
            @PrimaryLeft.started -= instance.OnPrimaryLeft;
            @PrimaryLeft.performed -= instance.OnPrimaryLeft;
            @PrimaryLeft.canceled -= instance.OnPrimaryLeft;
            @PrimaryRight.started -= instance.OnPrimaryRight;
            @PrimaryRight.performed -= instance.OnPrimaryRight;
            @PrimaryRight.canceled -= instance.OnPrimaryRight;
            @SecondaryLeft.started -= instance.OnSecondaryLeft;
            @SecondaryLeft.performed -= instance.OnSecondaryLeft;
            @SecondaryLeft.canceled -= instance.OnSecondaryLeft;
            @SecondaryRight.started -= instance.OnSecondaryRight;
            @SecondaryRight.performed -= instance.OnSecondaryRight;
            @SecondaryRight.canceled -= instance.OnSecondaryRight;
            @GripLeft.started -= instance.OnGripLeft;
            @GripLeft.performed -= instance.OnGripLeft;
            @GripLeft.canceled -= instance.OnGripLeft;
            @GripRight.started -= instance.OnGripRight;
            @GripRight.performed -= instance.OnGripRight;
            @GripRight.canceled -= instance.OnGripRight;
            @TriggerLeft.started -= instance.OnTriggerLeft;
            @TriggerLeft.performed -= instance.OnTriggerLeft;
            @TriggerLeft.canceled -= instance.OnTriggerLeft;
            @TriggerRight.started -= instance.OnTriggerRight;
            @TriggerRight.performed -= instance.OnTriggerRight;
            @TriggerRight.canceled -= instance.OnTriggerRight;
        }

        public void RemoveCallbacks(IPatcherActions instance)
        {
            if (m_Wrapper.m_PatcherActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPatcherActions instance)
        {
            foreach (var item in m_Wrapper.m_PatcherActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PatcherActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PatcherActions @Patcher => new PatcherActions(this);
    public interface IPatcherActions
    {
        void OnPrimaryLeft(InputAction.CallbackContext context);
        void OnPrimaryRight(InputAction.CallbackContext context);
        void OnSecondaryLeft(InputAction.CallbackContext context);
        void OnSecondaryRight(InputAction.CallbackContext context);
        void OnGripLeft(InputAction.CallbackContext context);
        void OnGripRight(InputAction.CallbackContext context);
        void OnTriggerLeft(InputAction.CallbackContext context);
        void OnTriggerRight(InputAction.CallbackContext context);
    }
}