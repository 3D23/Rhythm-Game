using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction rhythmSpeedAction;
    private InputAction accelerationAction;
    private InputAction moveAction;
    private InputAction modeAction;

    public Action OnPositiveRhythmSpeedAction;
    public Action OnNegativeRhythmSpeedAction;
    public Action<bool> OnPositiveAccelerationAction;
    public Action<bool> OnNegativeAccelerationAction;
    public Action<bool> OnPositiveMoveAction;
    public Action<bool> OnNegativeMoveAction;
    public Action OnPositiveSpeedModeAction;
    public Action OnNegativeSpeedModeAction;

    #region Unity Lifecycle
    private void Awake()
    {
        var p = inputActions.FindActionMap("P");
        rhythmSpeedAction = p.FindAction("RhythmSpeed");
        accelerationAction = p.FindAction("Acceleration");
        moveAction = p.FindAction("Move");
        modeAction = p.FindAction("Mode");
    }

    private void OnEnable()
    {
        rhythmSpeedAction.Enable();
        accelerationAction.Enable();
        modeAction.Enable();
        moveAction.Enable();

        rhythmSpeedAction.started += OnSpeedActionStart;
        accelerationAction.started += OnAccelerationActionStart;
        accelerationAction.canceled += OnAccelerationActionCancel;
        moveAction.performed += OnMoveAction;
        moveAction.canceled += OnMoveAction;
        modeAction.canceled += OnModeActionCancel;
    }

    private void OnDisable()
    {
        rhythmSpeedAction.performed -= OnSpeedActionStart;
        accelerationAction.started -= OnAccelerationActionStart;
        accelerationAction.canceled -= OnAccelerationActionCancel;
        moveAction.performed -= OnMoveAction;
        moveAction.canceled -= OnMoveAction;
        modeAction.canceled -= OnModeActionCancel;

        rhythmSpeedAction.Disable();
        accelerationAction.Disable();
        modeAction.Disable();
        moveAction.Disable();
    }
    #endregion

    private void OnSpeedActionStart(InputAction.CallbackContext context)
    {
        InputBinding binding = context.action.bindings[context.action.GetBindingIndexForControl(context.control)];
        if (binding.name.ToLower() == "positive")
        {
            OnPositiveRhythmSpeedAction?.Invoke();
        }
        else if (binding.name.ToLower() == "negative")
        {
            OnNegativeRhythmSpeedAction?.Invoke();
        }
    }

    private void OnModeActionCancel(InputAction.CallbackContext context)
    {
        InputBinding binding = context.action.bindings[context.action.GetBindingIndexForControl(context.control)];
        if (binding.name.ToLower() == "positive") OnPositiveSpeedModeAction?.Invoke();
        else if (binding.name.ToLower() == "negative") OnNegativeSpeedModeAction?.Invoke();
    }

    private void OnMoveAction(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        if (vector.y > 0)
        {
            OnPositiveMoveAction?.Invoke(true);
            OnNegativeMoveAction?.Invoke(false);
        }
        else if (vector.y < 0)
        {
            OnPositiveMoveAction?.Invoke(false);
            OnNegativeMoveAction?.Invoke(true);
        }
        else
        {
            OnPositiveMoveAction?.Invoke(false);
            OnNegativeMoveAction?.Invoke(false);
        }
    }

    private void OnAccelerationActionCancel(InputAction.CallbackContext context)
    {
        InputBinding binding = context.action.bindings[context.action.GetBindingIndexForControl(context.control)];
        if (binding.name.ToLower() == "positive") OnPositiveAccelerationAction?.Invoke(false);
        else if (binding.name.ToLower() == "negative") OnNegativeAccelerationAction?.Invoke(false);
    }

    private void OnAccelerationActionStart(InputAction.CallbackContext context)
    {
        InputBinding binding = context.action.bindings[context.action.GetBindingIndexForControl(context.control)];
        if (binding.name.ToLower() == "positive") OnPositiveAccelerationAction?.Invoke(true);
        else if (binding.name.ToLower() == "negative") OnNegativeAccelerationAction?.Invoke(true);
    }
}