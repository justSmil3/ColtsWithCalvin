// GENERATED AUTOMATICALLY FROM 'Assets/Input/SimpleCharImput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @SimpleCharImput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @SimpleCharImput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SimpleCharImput"",
    ""maps"": [
        {
            ""name"": ""PlayerOne"",
            ""id"": ""f6d5e068-3fea-4e05-bed7-845b0d5530ca"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f74e2f29-5994-4f9c-983e-b5574884de0c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""0e6fb5f7-a44e-495b-8fc5-9471dee6ed03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMove"",
                    ""type"": ""Value"",
                    ""id"": ""fd3edce2-58f6-4f32-b98b-36de2a8bb61e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""56300818-92d8-4019-aeba-d4036fe4b368"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a49a71e4-c668-45e4-8dcb-eb94c2a69ece"",
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
                    ""id"": ""daa7ce52-406c-42a8-8fcd-776a3285cd8b"",
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
                    ""id"": ""bb62dd81-477f-4017-8cb6-117e471b9260"",
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
                    ""id"": ""34f6dc21-ca6d-43d3-8b94-b55e4b954940"",
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
                    ""id"": ""59e062c9-f31d-40e5-b2e4-ab1e073327bb"",
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
                    ""id"": ""70932a64-927a-40a8-9e0d-6570bc6d3400"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerTwo"",
            ""id"": ""a1b9895a-9755-4c3d-b74f-2c74b3034a00"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""17cde9e0-a2de-469a-9a65-f35190882804"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a887bf47-7e4e-4562-9763-37644964d35d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMove"",
                    ""type"": ""Value"",
                    ""id"": ""4adb9ed5-5981-4cb3-93fa-d4a043f408bb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""538abf6d-827b-4376-9829-e76246c1f357"",
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
                    ""id"": ""a4a2e1c4-009c-4eb3-8aec-b3d624e31416"",
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
                    ""id"": ""4fb95b43-cf49-4d32-8b14-10f113ebe01e"",
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
                    ""id"": ""53c1b924-b084-468d-a43a-5618429c0a71"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerOne
        m_PlayerOne = asset.FindActionMap("PlayerOne", throwIfNotFound: true);
        m_PlayerOne_Move = m_PlayerOne.FindAction("Move", throwIfNotFound: true);
        m_PlayerOne_Jump = m_PlayerOne.FindAction("Jump", throwIfNotFound: true);
        m_PlayerOne_CameraMove = m_PlayerOne.FindAction("CameraMove", throwIfNotFound: true);
        // PlayerTwo
        m_PlayerTwo = asset.FindActionMap("PlayerTwo", throwIfNotFound: true);
        m_PlayerTwo_Move = m_PlayerTwo.FindAction("Move", throwIfNotFound: true);
        m_PlayerTwo_Jump = m_PlayerTwo.FindAction("Jump", throwIfNotFound: true);
        m_PlayerTwo_CameraMove = m_PlayerTwo.FindAction("CameraMove", throwIfNotFound: true);
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

    // PlayerOne
    private readonly InputActionMap m_PlayerOne;
    private IPlayerOneActions m_PlayerOneActionsCallbackInterface;
    private readonly InputAction m_PlayerOne_Move;
    private readonly InputAction m_PlayerOne_Jump;
    private readonly InputAction m_PlayerOne_CameraMove;
    public struct PlayerOneActions
    {
        private @SimpleCharImput m_Wrapper;
        public PlayerOneActions(@SimpleCharImput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerOne_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerOne_Jump;
        public InputAction @CameraMove => m_Wrapper.m_PlayerOne_CameraMove;
        public InputActionMap Get() { return m_Wrapper.m_PlayerOne; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerOneActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerOneActions instance)
        {
            if (m_Wrapper.m_PlayerOneActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnJump;
                @CameraMove.started -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnCameraMove;
                @CameraMove.performed -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnCameraMove;
                @CameraMove.canceled -= m_Wrapper.m_PlayerOneActionsCallbackInterface.OnCameraMove;
            }
            m_Wrapper.m_PlayerOneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @CameraMove.started += instance.OnCameraMove;
                @CameraMove.performed += instance.OnCameraMove;
                @CameraMove.canceled += instance.OnCameraMove;
            }
        }
    }
    public PlayerOneActions @PlayerOne => new PlayerOneActions(this);

    // PlayerTwo
    private readonly InputActionMap m_PlayerTwo;
    private IPlayerTwoActions m_PlayerTwoActionsCallbackInterface;
    private readonly InputAction m_PlayerTwo_Move;
    private readonly InputAction m_PlayerTwo_Jump;
    private readonly InputAction m_PlayerTwo_CameraMove;
    public struct PlayerTwoActions
    {
        private @SimpleCharImput m_Wrapper;
        public PlayerTwoActions(@SimpleCharImput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerTwo_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerTwo_Jump;
        public InputAction @CameraMove => m_Wrapper.m_PlayerTwo_CameraMove;
        public InputActionMap Get() { return m_Wrapper.m_PlayerTwo; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerTwoActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerTwoActions instance)
        {
            if (m_Wrapper.m_PlayerTwoActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnJump;
                @CameraMove.started -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnCameraMove;
                @CameraMove.performed -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnCameraMove;
                @CameraMove.canceled -= m_Wrapper.m_PlayerTwoActionsCallbackInterface.OnCameraMove;
            }
            m_Wrapper.m_PlayerTwoActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @CameraMove.started += instance.OnCameraMove;
                @CameraMove.performed += instance.OnCameraMove;
                @CameraMove.canceled += instance.OnCameraMove;
            }
        }
    }
    public PlayerTwoActions @PlayerTwo => new PlayerTwoActions(this);
    public interface IPlayerOneActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCameraMove(InputAction.CallbackContext context);
    }
    public interface IPlayerTwoActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCameraMove(InputAction.CallbackContext context);
    }
}
