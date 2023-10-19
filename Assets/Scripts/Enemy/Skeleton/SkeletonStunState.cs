using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : SkeletonState
{
    public SkeletonStunState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.stunTimer;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
