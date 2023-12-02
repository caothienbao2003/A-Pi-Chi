using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public GameInputActions gameInputActions { get; private set; }
    public event EventHandler OnJumpPress;
    public event EventHandler OnJumpRelease;
    public event EventHandler OnDashPress;
    public event EventHandler OnAttackPress;
    public event EventHandler OnAttackRelease;
    public event EventHandler OnGuardPress;
    public event EventHandler OnSwordSkillPress;
    public event EventHandler OnSwordSkillRelease;
    public event EventHandler OnUltimateSkillPress;
    public event EventHandler OnCrystalSkillPress;

    public event EventHandler OnTouchScreen;

    public static GameInput instance;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        instance = this;

        gameInputActions = new GameInputActions();


        gameInputActions.Player.Enable();
        gameInputActions.Touch.Enable();

        gameInputActions.Player.Jump.performed += Jump_performed;
        gameInputActions.Player.Jump.canceled += Jump_canceled;
        gameInputActions.Player.Dash.performed += Dash_performed;
        gameInputActions.Player.Attack.performed += Attack_performed;
        gameInputActions.Player.Attack.canceled += Attack_canceled;
        gameInputActions.Player.Guard.performed += Guard_performed;
        gameInputActions.Player.SwordSkill.performed += SwordSkill_performed;
        gameInputActions.Player.SwordSkill.canceled += SwordSkill_canceled;
        gameInputActions.Player.UltimateSkill.performed += UltimateSkill_performed;
        gameInputActions.Player.CrystalSkill.performed += CrystalSkill_performed;

        gameInputActions.Touch.PrimaryTouch.performed += PrimaryTouch_performed;
    }

    private void CrystalSkill_performed(InputAction.CallbackContext obj)
    {
        OnCrystalSkillPress?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        gameInputActions.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Disable();
    }

    private void UltimateSkill_performed(InputAction.CallbackContext obj)
    {
        OnUltimateSkillPress?.Invoke(this, EventArgs.Empty);
    }

    private void PrimaryTouch_performed(InputAction.CallbackContext obj)
    {
        OnTouchScreen?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_canceled(InputAction.CallbackContext obj)
    {
        OnAttackRelease?.Invoke(this, EventArgs.Empty);
    }

    private void SwordSkill_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwordSkillRelease?.Invoke(this, EventArgs.Empty);
    }

    private void SwordSkill_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwordSkillPress?.Invoke(this, EventArgs.Empty);
    }

    private void Guard_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGuardPress?.Invoke(this, EventArgs.Empty);
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

    public Vector2 GetAimDirectionInput()
    {
        return -gameInputActions.Player.AimDirection.ReadValue<Vector2>();
    }

    public Vector2 GetTouchWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(gameInputActions.Touch.TouchPosition.ReadValue<Vector2>());
    }

    public GameObject GetTouchedGameObject(LayerMask layer)
    {
        GameObject target = null;
        Ray ray = mainCamera.ScreenPointToRay(gameInputActions.Touch.TouchPosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, layer);

        if (hit)
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}
