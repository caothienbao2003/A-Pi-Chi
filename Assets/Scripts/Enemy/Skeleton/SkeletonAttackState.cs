using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : SkeletonState
{
    public SkeletonAttackState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skeleton.HandleFlip(skeleton.IsDetectingPlayer().transform.position.x - skeleton.transform.position.x);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.ChangeState(skeleton.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.CannotBeStuned();
    }
}
