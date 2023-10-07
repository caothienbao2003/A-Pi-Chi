using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    #region State
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonWalkState walkState { get; private set; }
    public SkeletonReactState reactState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        walkState = new SkeletonWalkState(this, stateMachine, "Walk");
        reactState = new SkeletonReactState(this, stateMachine, "React");
        battleState = new SkeletonBattleState(this, stateMachine, "Walk");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");

        stateMachine.Initialize(idleState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
    }
}
