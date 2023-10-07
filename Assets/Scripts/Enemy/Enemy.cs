using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
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

    protected override void Awake()
    {
        base.Awake();
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

    public virtual Collider2D IsDetectingPlayer()
    {
        return Physics2D.OverlapBox(playerCheck.position, playerCheckBoxSize, 0f, playerLayer);
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(playerCheck.position, playerCheckBoxSize);
    }
}
