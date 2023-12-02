using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Entity
{
    #region State
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerWallHopState wallHopState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerHitState hitState { get; private set; }
    public PlayerGuardState guardState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerWaitSwordState waitSwordState { get; private set; }
    public PlayerThrowSwordState throwSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerUltimateState ultimateState { get; private set; }
    #endregion

    #region Reference
    private GameInput gameInput;
    private SkillManager skillManager;
    #endregion

    #region Parameters
    [Header("Move On Ground")]
    public float moveSpeed;
    public float groundSlideMultiplier;
    public float xInput { get; private set; }
    public float yInput { get; private set; }

    [Header("Move On Air")]
    public float jumpForce;
    public float movementForceInAir;
    public float airDragMultiplier;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float coyoteTime = .2f;
    [SerializeField] private float maxFallSpeed;

    private float coyoteTimeCounter;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 0.5f;
    public float wallSlideHoldTime;

    [Header("Wall Jump")]
    public float wallJumpTime;
    public float wallJumpForce;
    public Vector2 wallJumpDir;

    [Header("Wall Hop")]
    public float wallHopTime;
    public float wallHopForce;
    public Vector2 wallHopDir;

    [Header("Attack")]
    public float comboTime;
    public Vector2[] attackMovements;
    public Vector2[] knockBackAttackDir;
    public float[] knockBackAttackForce;

    [Header("Counter Attack Info")]
    public float counterAttackTimer;
    public float counterAttackCooldown;
    public Vector2 counterAttackKnockbackDir;
    public float counterAttackKnockbackForce;

    [Header("Sword Skill")]
    public float cancelAimInputRange = .3f;
    public Vector2 cancelAimInputArea { get; set; }
    public Vector2 aimSwordDirectionInput { get; set; }

    private LayerMask enemyLayer;
    #endregion

    #region Variables
    public bool isBusy { get; private set; }
    private bool canGuard = true;
    public bool isHoldingAttackButton { get; set; }
    #endregion

    [SerializeField] private GameObject moveButton;
    [SerializeField] private GameObject skillButton;

    protected override void Awake()
    {
        base.Awake();
        SetUpState();
    }

    private void SetUpState()
    {
        groundedState = new PlayerGroundedState(this, stateMachine, "Grounded");
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        fallState = new PlayerFallState(this, stateMachine, "Fall");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "Wall Slide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        wallHopState = new PlayerWallHopState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        hitState = new PlayerHitState(this, stateMachine, "Hit");
        guardState = new PlayerGuardState(this, stateMachine, "Guard");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "ThrowSword");
        waitSwordState = new PlayerWaitSwordState(this, stateMachine, "WaitSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        ultimateState = new PlayerUltimateState(this, stateMachine, "Fall");
    }


    #region GameInput
    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        if (IsCurrentStateEqualTo(primaryAttackState)
            || IsCurrentStateEqualTo(aimSwordState)
            || IsCurrentStateEqualTo(throwSwordState)
            || IsCurrentStateEqualTo(catchSwordState)
            || IsCurrentStateEqualTo(waitSwordState)
            || IsCurrentStateEqualTo(ultimateState))
        {
            return;
        }

        if (!SkillManager.instance.swordSkill.IsAllSwordThrown())
        {
            aimSwordDirectionInput = Vector2.zero;
            stateMachine.ChangeState(aimSwordState);
        }
        else
        {
            stateMachine.ChangeState(waitSwordState);
        }

    }
    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        if (IsCurrentStateEqualTo(aimSwordState))
        {
            if (IsAimInputInCancelRange())
            {
                stateMachine.ChangeState(catchSwordState);
            }
            else
            {
                stateMachine.ChangeState(throwSwordState);
            }
        }
    }
    private void GameInput_OnAttackPress(object sender, System.EventArgs e)
    {
        isHoldingAttackButton = true;

        if (IsCurrentStateEqualTo(primaryAttackState)
            || IsCurrentStateEqualTo(hitState)
            || IsCurrentStateEqualTo(catchSwordState)
            || IsCurrentStateEqualTo(throwSwordState)
            || IsCurrentStateEqualTo(waitSwordState)
            || IsCurrentStateEqualTo(ultimateState)
            || IsCurrentStateEqualTo(wallSlideState)
            )
        {
            return;
        }


        stateMachine.ChangeState(primaryAttackState);
    }
    private void GameInput_OnAttackRelease(object sender, System.EventArgs e)
    {
        isHoldingAttackButton = false;
    }
    private void GameInput_OnDashPress(object sender, System.EventArgs e)
    {
        if (IsCurrentStateEqualTo(dashState)
            || IsCurrentStateEqualTo(wallSlideState)
            || IsCurrentStateEqualTo(aimSwordState)
            || IsCurrentStateEqualTo(throwSwordState)
            || IsCurrentStateEqualTo(waitSwordState)
            || IsCurrentStateEqualTo(ultimateState))
        {
            return;
        }

        if (!skillManager.dashSkill.CanUseSkill())
        {
            return;
        }

        skillManager.dashSkill.ResetCoolDownTimer();
        stateMachine.ChangeState(dashState);

    }
    private void GameInput_OnJumpPress(object sender, System.EventArgs e)
    {
        if (IsCurrentStateEqualTo(primaryAttackState)
            || IsCurrentStateEqualTo(aimSwordState)
            || IsCurrentStateEqualTo(throwSwordState)
            || IsCurrentStateEqualTo(catchSwordState)
            || IsCurrentStateEqualTo(waitSwordState)
            || IsCurrentStateEqualTo(dashState)
            || IsCurrentStateEqualTo(ultimateState))
        {
            return;
        }

        if (coyoteTimeCounter > 0)
        {
            stateMachine.ChangeState(jumpState);
        }
        else if (IsCurrentStateEqualTo(wallSlideState))
        {
            stateMachine.ChangeState(wallJumpState); 
        }
    }
    private void GameInput_OnJumpRelease(object sender, System.EventArgs e)
    {
        coyoteTimeCounter = 0;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
    }
    private void GameInput_OnGuardPress(object sender, System.EventArgs e)
    {
        if (IsCurrentStateEqualTo(guardState)
            || IsCurrentStateEqualTo(counterAttackState)
            || IsCurrentStateEqualTo(hitState)
            || IsCurrentStateEqualTo(aimSwordState)
            || IsCurrentStateEqualTo(throwSwordState)
            || IsCurrentStateEqualTo(catchSwordState)
            || IsCurrentStateEqualTo(waitSwordState)
            || IsCurrentStateEqualTo(ultimateState))
        {
            return;
        }

        if(!skillManager.parrySkill.CanUseSkill())
        {
            return;
        }

        if (canGuard)
        {
            stateMachine.ChangeState(guardState);
        }
    }
    private void GameInput_OnUltimateSkillPress(object sender, System.EventArgs e)
    {
        if(IsCurrentStateEqualTo(ultimateState))
        {
            return;
        }

        if(!skillManager.ultimateSkill.CanUseSkill())
        {
            return;
        }
        stateMachine.ChangeState(ultimateState);
    }
    private void GameInput_OnCrystalSkillPress(object sender, System.EventArgs e)
    {
        
    }
    private void GameInput_OnTouchScreen(object sender, System.EventArgs e)
    {
        
    }
    #endregion

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        coyoteTimeCounter = coyoteTime;

        enemyLayer = GameManager.instance.enemyLayer;
        skillManager = SkillManager.instance;

        InitializeGameInput();
    }

    private void InitializeGameInput()
    {
        gameInput = GameInput.instance;

        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnJumpRelease += GameInput_OnJumpRelease;
        gameInput.OnDashPress += GameInput_OnDashPress;
        gameInput.OnAttackPress += GameInput_OnAttackPress;
        gameInput.OnAttackRelease += GameInput_OnAttackRelease;
        gameInput.OnGuardPress += GameInput_OnGuardPress;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
        gameInput.OnUltimateSkillPress += GameInput_OnUltimateSkillPress;
        gameInput.OnCrystalSkillPress += GameInput_OnCrystalSkillPress;

        gameInput.OnTouchScreen += GameInput_OnTouchScreen;
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        GetHorizontalInput();
        GetAimDirectionInput();

        ClampVelocity();
        HandleCoyoteTime();
    }

    private void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else if (coyoteTimeCounter >= 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void GetAimDirectionInput()
    {
        if (gameInput.GetAimDirectionInput() != Vector2.zero)
        {
            aimSwordDirectionInput = gameInput.GetAimDirectionInput();
            FaceTo(aimSwordDirectionInput.x);
        }
    }

    private void GetHorizontalInput()
    {
        xInput = gameInput.GetXInput();
        yInput = gameInput.GetYInput();
    }

    private void ClampVelocity()
    {
        Utilities.ClampVelocity(rb, -maxFallSpeed, float.MaxValue);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }

    #region Busy
    private IEnumerator BusyForCo(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void BusyFor(float seconds)
    {
        StartCoroutine(BusyForCo(seconds));
    }
    #endregion

    #region ChangeState
    public override void ChangeToHitState()
    {
        base.ChangeToHitState();

        stateMachine.ChangeState(hitState);
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
    }
    #endregion

    #region Detect Enemy
    public Collider2D[] EnemiesDectected()
    {
        return Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, enemyLayer);
    }
    #endregion

    #region Check Current State
    public bool IsCurrentStateEqualTo(PlayerState state)
    {
        return stateMachine.currentState == state;
    }
    #endregion

    public bool IsAimInputInCancelRange()
    {
        return Mathf.Abs(aimSwordDirectionInput.x) <= cancelAimInputRange
            && Mathf.Abs(aimSwordDirectionInput.y) <= cancelAimInputRange;
    }
}