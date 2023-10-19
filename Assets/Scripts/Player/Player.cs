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

    #endregion

    #region Reference
    private GameInput gameInput;
    #endregion

    #region Parameters
    [Header("Move On Ground")]
    public float moveSpeed;
    public float xInput { get; private set; }
    public float yInput { get; private set; }

    [Header("Move On Air")]
    public float jumpForce;
    public float movementForceInAir;
    public float airDragMultiplier;
    [SerializeField] private float jumpHeightMultiplier;

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
    public LayerMask enemyLayer;
    public float counterAttackCooldown;
    public Vector2 counterAttackKnockbackDir;
    public float counterAttackKnockbackForce;

    #endregion

    #region Variables
    public bool isBusy { get; private set; }
    private bool canGuard = true;
    #endregion

    protected override void Awake()
    {
        base.Awake();

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

        gameInput = GameInput.instance;
        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnJumpRelease += GameInput_OnJumpRelease;
        gameInput.OnDashPress += GameInput_OnDashPress;
        gameInput.OnAttackPress += GameInput_OnAttackPress;
        gameInput.OnGuardPress += GameInput_OnGuardPress;
    }

    #region GameInput
    private void GameInput_OnAttackPress(object sender, System.EventArgs e)
    {
        if (stateMachine.currentState != primaryAttackState && stateMachine.currentState != hitState)
        {
            stateMachine.ChangeState(primaryAttackState);
        }
    }

    private void GameInput_OnDashPress(object sender, System.EventArgs e)
    {
        if (stateMachine.currentState != dashState && stateMachine.currentState != wallSlideState && SkillManager.instance.dashSkill.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    private void GameInput_OnJumpPress(object sender, System.EventArgs e)
    {
        if (IsGrounded() && stateMachine.currentState != dashState)
        {
            stateMachine.ChangeState(jumpState);
        }
        if (stateMachine.currentState == wallSlideState)
        {
            if (xInput == 0)
            {
                stateMachine.ChangeState(wallHopState);
            }
            else if (xInput != transform.right.x)
            {
                stateMachine.ChangeState(wallJumpState);
            }
        }
    }

    private void GameInput_OnJumpRelease(object sender, System.EventArgs e)
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
    }
    private void GameInput_OnGuardPress(object sender, System.EventArgs e)
    {
        if (stateMachine.currentState != guardState && stateMachine.currentState != counterAttackState && stateMachine.currentState != hitState && canGuard)
        {
            stateMachine.ChangeState(guardState);
        }
    }

    #endregion

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        xInput = gameInput.GetXInput();
        yInput = gameInput.GetYInput();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }


    #region CoolDown

    private IEnumerator GuardCoolDownCo()
    {
        canGuard = false;
        yield return new WaitForSeconds(counterAttackCooldown);
        canGuard = true;
    }

    public void GuardCoolDown()
    {
        StartCoroutine(GuardCoolDownCo());
    }

    #endregion

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
    #endregion

    #region Detect Enemy
    public Collider2D[] EnemiesDectected()
    {
        return Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, enemyLayer);
    }
    #endregion
}