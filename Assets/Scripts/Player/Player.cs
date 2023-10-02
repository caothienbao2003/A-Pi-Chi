using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region State
    private PlayerStateMachine stateMachine;
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
    #endregion

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region Reference
    private GameInput gameInput;
    #endregion
    
    #region Parameters
    [Header("Move On Ground")]
    public float moveSpeed;
    public float xInput { get; private set; }
    public float yInput { get; private set;}

    [Header("Move On Air")]
    public float jumpForce;
    public float movementForceInAir;
    public float airDragMultiplier;
    [SerializeField] private float jumpHeightMultiplier;


    [Header("Collision")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    [SerializeField] private float dashCoodown;
    private bool canDash = true;

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

    #endregion

    #region Variables
    public bool isBusy { get; private set; }
    #endregion

    protected override void Awake()
    {
        stateMachine = new PlayerStateMachine();

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

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        gameInput = FindObjectOfType<GameInput>();
        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnJumpRelease += GameInput_OnJumpRelease;
        gameInput.OnDashPress += GameInput_OnDashPress;
        gameInput.OnAttackPress += GameInput_OnAttackPress;
    }

    private void GameInput_OnAttackPress(object sender, System.EventArgs e)
    {
        if(stateMachine.currentState != primaryAttackState)
        {
            stateMachine.ChangeState(primaryAttackState);
        }
    }

    private void GameInput_OnDashPress(object sender, System.EventArgs e)
    {
        if(stateMachine.currentState != dashState && stateMachine.currentState != wallSlideState && canDash)
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
        if(stateMachine.currentState == wallSlideState)
        {
            if(xInput == 0)
            {
                stateMachine.ChangeState(wallHopState);
            }
            else if(xInput != transform.right.x)
            {
                stateMachine.ChangeState(wallJumpState);
            }
        }
    }

    private void GameInput_OnJumpRelease(object sender, System.EventArgs e)
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
    }

    protected override void Start()
    {
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        stateMachine.currentState.Update();

        xInput = gameInput.GetXInput();
        yInput = gameInput.GetYInput();
    }

    protected override void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }

    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        HandleFlip(x);
    }

    public bool IsGrounded() => Physics2D.CircleCast(groundCheck.position, .2f, Vector2.down, groundCheckDistance, groundLayer);

    public bool IsTouchingWall() => Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void HandleFlip(float moveX)
    {
        if (moveX != 0)
        {
            float moveDir = moveX;
            transform.right = new Vector2(moveDir, 0);
        }
    }

    private IEnumerator DashCooldownCo()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCoodown);
        canDash = true;
    }

    public void DashCooldown()
    {
        StartCoroutine(DashCooldownCo());
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    
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
}