using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skeleton.idleTime;
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

        if(stateTimer < 0) 
        {
            if(skeleton.IsTouchingWall() || !skeleton.IsGrounded())
            {
                skeleton.FaceTo(-skeleton.transform.right.x);
                stateMachine.ChangeState(skeleton.idleState);
            }
            else
            {
                stateMachine.ChangeState(skeleton.walkState);
            }
        }
    }
}
