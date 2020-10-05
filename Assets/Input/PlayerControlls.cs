// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControlls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControlls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControlls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControlls"",
    ""maps"": [
        {
            ""name"": ""PlayerOneControlls"",
            ""id"": ""8b0cdef7-c0c8-43f4-b19b-e223c36a6fd1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""72f6aff3-28a2-42ca-b7e8-463177d3ff74"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a512d1a2-8215-4ef5-9248-d0d3dd895d00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InitiateRope"",
                    ""type"": ""Button"",
                    ""id"": ""a05b552f-9241-4953-bc02-1f5f1c822a92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""084ef7c2-1759-41f0-9f22-ae67f3b905ee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fbb00275-d79d-42f4-83eb-4eb44eea9c6e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0ec2d1ed-7f1e-405e-af9f-5a117a43ca64"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""528fcf27-d69c-4440-9cf2-c36911cba1cf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9bb00440-f9b7-4de8-a9a8-45e9b9c82206"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""80b229d0-b2e5-4eb3-9885-f65bbef75837"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2867ca66-a6ae-4744-a07c-dfb4b05d7e7f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e4854db-ae85-4165-9721-623b150653f5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InitiateRope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f6b564a-dfe8-428f-bfc8-e771aa231bf4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerTwoControlls"",
            ""id"": ""981ca7de-5f66-40de-8a5d-b9ef8105574a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ca50870e-91c3-4f19-b093-ac4a5ab1942f"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4f947265-6ef9-4c67-a186-cf085a176e70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InitiateRope"",
                    ""type"": ""Button"",
                    ""id"": ""9a3ba6d3-9f6c-4879-9495-8d4cec675674"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""9ea9db44-bdee-4b48-a3ef-3325df2ef9e2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d691f31-5dea-4e8f-8edf-9c02069ca5fc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9cc8624a-7cab-47b2-b297-f0c894b91690"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f84bca4-3336-40ff-a7e3-46bdbe206104"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56100d89-8be6-4fd9-a15c-bc06aa803837"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InitiateRope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a715e6d-af2b-45f1-adcd-e7d4f9b7010f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InitiateRope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c56bb89c-1f00-4f03-9eed-b13706d20751"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerOneControlls
        m_PlayerOneControlls = asset.FindActionMap("PlayerOneControlls", throwIfNotFound: true);
        m_PlayerOneControlls_Move = m_PlayerOneControlls.FindAction("Move", throwIfNotFound: true);
        m_PlayerOneControlls_Jump = m_PlayerOneControlls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerOneControlls_InitiateRope = m_PlayerOneControlls.FindAction("InitiateRope", throwIfNotFound: true);
        m_PlayerOneControlls_Camera = m_PlayerOneControlls.FindAction("Camera", throwIfNotFound: true);
        // PlayerTwoControlls
        m_PlayerTwoControlls = asset.FindActionMap("PlayerTwoControlls", throwIfNotFound: true);
        m_PlayerTwoControlls_Move = m_PlayerTwoControlls.FindAction("Move", throwIfNotFound: true);
        m_PlayerTwoControlls_Jump = m_PlayerTwoControlls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerTwoControlls_InitiateRope = m_PlayerTwoControlls.FindAction("InitiateRope", throwIfNotFound: true);
        m_PlayerTwoControlls_Camera = m_PlayerTwoControlls.FindAction("Camera", throwIfNotFound: true);
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

    // PlayerOneControlls
    private readonly InputActionMap m_PlayerOneControlls;
    private IPlayerOneControllsActions m_PlayerOneControllsActionsCallbackInterface;
    private readonly InputAction m_PlayerOneControlls_Move;
    private readonly InputAction m_PlayerOneControlls_Jump;
    private readonly InputAction m_PlayerOneControlls_InitiateRope;
    private readonly InputAction m_PlayerOneControlls_Camera;
    public struct PlayerOneControllsActions
    {
        private @PlayerControlls m_Wrapper;
        public PlayerOneControllsActions(@PlayerControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerOneControlls_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerOneControlls_Jump;
        public InputAction @InitiateRope => m_Wrapper.m_PlayerOneControlls_InitiateRope;
        public InputAction @Camera => m_Wrapper.m_PlayerOneControlls_Camera;
        public InputActionMap Get() { return m_Wrapper.m_PlayerOneControlls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerOneControllsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerOneControllsActions instance)
        {
            if (m_Wrapper.m_PlayerOneControllsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnJump;
                @InitiateRope.started -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnInitiateRope;
                @InitiateRope.performed -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnInitiateRope;
                @InitiateRope.canceled -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnInitiateRope;
                @Camera.started -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerOneControllsActionsCallbackInterface.OnCamera;
            }
            m_Wrapper.m_PlayerOneControllsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @InitiateRope.started += instance.OnInitiateRope;
                @InitiateRope.performed += instance.OnInitiateRope;
                @InitiateRope.canceled += instance.OnInitiateRope;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
            }
        }
    }
    public PlayerOneControllsActions @PlayerOneControlls => new PlayerOneControllsActions(this);

    // PlayerTwoControlls
    private readonly InputActionMap m_PlayerTwoControlls;
    private IPlayerTwoControllsActions m_PlayerTwoControllsActionsCallbackInterface;
    private readonly InputAction m_PlayerTwoControlls_Move;
    private readonly InputAction m_PlayerTwoControlls_Jump;
    private readonly InputAction m_PlayerTwoControlls_InitiateRope;
    private readonly InputAction m_PlayerTwoControlls_Camera;
    public struct PlayerTwoControllsActions
    {
        private @PlayerControlls m_Wrapper;
        public PlayerTwoControllsActions(@PlayerControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerTwoControlls_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerTwoControlls_Jump;
        public InputAction @InitiateRope => m_Wrapper.m_PlayerTwoControlls_InitiateRope;
        public InputAction @Camera => m_Wrapper.m_PlayerTwoControlls_Camera;
        public InputActionMap Get() { return m_Wrapper.m_PlayerTwoControlls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerTwoControllsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerTwoControllsActions instance)
        {
            if (m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnJump;
                @InitiateRope.started -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnInitiateRope;
                @InitiateRope.performed -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnInitiateRope;
                @InitiateRope.canceled -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnInitiateRope;
                @Camera.started -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface.OnCamera;
            }
            m_Wrapper.m_PlayerTwoControllsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @InitiateRope.started += instance.OnInitiateRope;
                @InitiateRope.performed += instance.OnInitiateRope;
                @InitiateRope.canceled += instance.OnInitiateRope;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
            }
        }
    }
    public PlayerTwoControllsActions @PlayerTwoControlls => new PlayerTwoControllsActions(this);
    public interface IPlayerOneControllsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInitiateRope(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
    }
    public interface IPlayerTwoControllsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInitiateRope(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
    }
}
