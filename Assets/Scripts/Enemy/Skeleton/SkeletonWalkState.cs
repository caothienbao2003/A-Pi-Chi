using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWalkState : SkeletonGroundedState
{
    public SkeletonWalkState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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

        enemy.SetVelocity(skeleton.moveSpeed * skeleton.transform.right.x, skeleton.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(skeleton.IsTouchingWall() || !skeleton.IsGrounded())
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
    }
}
