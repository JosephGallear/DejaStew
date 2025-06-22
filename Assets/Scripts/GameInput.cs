using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GameInput : MonoBehaviour, IHasInput
{
    public static GameInput Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnUseAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnRecordAction;
    public event EventHandler OnDeleteAction;
    public event EventHandler OnBindingRebind;
    public event EventHandler OnInputInTutorial;

    public enum Binding { Move_Up, Move_Down, Move_Left, Move_Right, Interact, Use, Record, Delete, Gamepad_Interact, Gamepad_Use, Gamepad_Record, Gamepad_Delete }
    private InputSystem_Actions playerInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        playerInputActions = new InputSystem_Actions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Use.performed += Use_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Record.performed += Record_performed;
        playerInputActions.Player.Delete.performed += Delete_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Use.performed -= Use_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Record.performed -= Record_performed;
        playerInputActions.Player.Delete.performed -= Delete_performed;

        playerInputActions.Dispose();
    }

    private void Delete_performed(InputAction.CallbackContext obj)
    {
        OnDeleteAction?.Invoke(this, EventArgs.Empty);
    }

    private void Record_performed(InputAction.CallbackContext obj)
    {
        OnRecordAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Use_performed(InputAction.CallbackContext obj)
    {
        OnUseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    private void Update()
    {
        if (GameManager.Instance.IsTutorialActive())
        {
            if (GetAnyInput())
            {
                OnInputInTutorial.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool GetAnyInput()
    {
        if (Keyboard.current != null)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if (key != null)
                {
                    if (key.wasPressedThisFrame)
                        return true;
                }
            }
        }

        if (Gamepad.current != null)
        {
            var gamepad = Gamepad.current;
            if (gamepad.buttonSouth.wasPressedThisFrame ||
                gamepad.buttonNorth.wasPressedThisFrame ||
                gamepad.buttonEast.wasPressedThisFrame ||
                gamepad.buttonWest.wasPressedThisFrame ||
                gamepad.startButton.wasPressedThisFrame ||
                gamepad.selectButton.wasPressedThisFrame ||
                gamepad.leftShoulder.wasPressedThisFrame ||
                gamepad.rightShoulder.wasPressedThisFrame ||
                gamepad.leftStickButton.wasPressedThisFrame ||
                gamepad.rightStickButton.wasPressedThisFrame ||
                gamepad.dpad.up.wasPressedThisFrame ||
                gamepad.dpad.down.wasPressedThisFrame ||
                gamepad.dpad.left.wasPressedThisFrame ||
                gamepad.dpad.right.wasPressedThisFrame)
            {
                return true;
            }
        }

        return false;
    }

    public bool GetInteractPressed() => playerInputActions.Player.Interact.triggered;
    public bool GetUsePressed() => playerInputActions.Player.Use.triggered;

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Use:
                return playerInputActions.Player.Use.bindings[0].ToDisplayString();
            case Binding.Record:
                return playerInputActions.Player.Record.bindings[0].ToDisplayString();
            case Binding.Delete:
                return playerInputActions.Player.Delete.bindings[1].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_Use:
                return playerInputActions.Player.Use.bindings[1].ToDisplayString();
            case Binding.Gamepad_Record:
                return playerInputActions.Player.Record.bindings[1].ToDisplayString();
            case Binding.Gamepad_Delete:
                return playerInputActions.Player.Delete.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;

            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;

            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;

            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.Use:
                inputAction = playerInputActions.Player.Use;
                bindingIndex = 0;
                break;

            case Binding.Record:
                inputAction = playerInputActions.Player.Record;
                bindingIndex = 0;
                break;

            case Binding.Delete:
                inputAction = playerInputActions.Player.Delete;
                bindingIndex = 0;
                break;

            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_Use:
                inputAction = playerInputActions.Player.Use;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_Record:
                inputAction = playerInputActions.Player.Record;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_Delete:
                inputAction = playerInputActions.Player.Delete;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
