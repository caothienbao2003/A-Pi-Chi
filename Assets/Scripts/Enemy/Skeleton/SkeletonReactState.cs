using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonReactState : SkeletonState
{
    public SkeletonReactState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
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

        if (triggerCalled)
        {
            if (skeleton.IsDetectingPlayer())
            {
                stateMachine.ChangeState(skeleton.battleState);
            }
            else
            {
                stateMachine.ChangeState(skeleton.idleState);
            }
        }
    }
}
