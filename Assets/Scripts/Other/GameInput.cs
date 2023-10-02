using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private GameInputActions gameInputActions;
    public event EventHandler OnJumpPress;
    public event EventHandler OnJumpRelease;
    public event EventHandler OnDashPress;
    public event EventHandler OnAttackPress;

    private void Awake()
    {
        gameInputActions = new GameInputActions();
        gameInputActions.Player.Enable();

        gameInputActions.Player.Jump.performed += Jump_performed;
        gameInputActions.Player.Jump.canceled += Jump_canceled;
        gameInputActions.Player.Dash.performed += Dash_performed;
        gameInputActions.Player.Attack.performed += Attack_performed;
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpRelease?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttackPress?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDashPress?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpPress?.Invoke(this, EventArgs.Empty);
    }

    public float GetXInput()
    {
        return gameInputActions.Player.XInput.ReadValue<float>();
    }

    public float GetYInput()
    {
        return gameInputActions.Player.YInput.ReadValue<float>();
    }
}
