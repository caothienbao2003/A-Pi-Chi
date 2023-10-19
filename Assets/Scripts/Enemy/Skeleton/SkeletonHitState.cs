using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHitState : SkeletonState
{
    public SkeletonHitState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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

        if(triggerCalled)
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
    }
}
