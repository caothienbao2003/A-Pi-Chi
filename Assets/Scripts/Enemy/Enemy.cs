using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region Paramaters
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleMoveSpeed;

    [Header("Collision Info")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Vector2 playerCheckBoxSize;

    [Header("Attack Info")]
    [SerializeField] protected LayerMask playerLayer;
    public float attackDistance;

    [Header("Knock back info")]
    [SerializeField] protected Vector2 knockBackDir;
    [SerializeField] protected float knockBackForce;

    [Header("Stun info")]
    public float stunTimer;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        currentKnockBackDir = knockBackDir;
        currentKnockBackForce = knockBackForce;
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.currentState.Enter();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.currentState.FixedUpdate();
    }

    #region Detect Player
    public virtual Collider2D IsDetectingPlayer()
    {
        return Physics2D.OverlapBox(playerCheck.position, playerCheckBoxSize, 0f, playerLayer);
    }
    #endregion

    #region Draw Gizmos
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(playerCheck.position, playerCheckBoxSize);
    }
    #endregion

    public virtual void ChangeToStunState()
    {

    }

    public virtual void CanBeStuned()
    {
        canBeStunned = true;
    }

    public virtual void CannotBeStuned()
    {
        canBeStunned = false;
    }
}
