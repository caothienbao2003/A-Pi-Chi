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

        skeleton.FaceTo(skeleton.IsDetectingPlayer().transform.position.x - skeleton.transform.position.x);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.CannotBeStuned();
    }

    public override void WhenFinishAnimation()
    {
        base.WhenFinishAnimation();

        stateMachine.ChangeState(skeleton.battleState);

    }
}
