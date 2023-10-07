using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : SkeletonState
{
    public SkeletonGroundedState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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

        if(skeleton.IsDetectingPlayer() && stateMachine.currentState!=skeleton.battleState)
        {
            stateMachine.ChangeState(skeleton.reactState);
        }
    }
}
