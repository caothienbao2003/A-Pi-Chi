using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.currentKnockBackDir = player.counterAttackKnockbackDir;
        player.currentKnockBackForce = player.counterAttackKnockbackForce;
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
    }

    public override void WhenFinishAnimation()
    {
        base.WhenFinishAnimation();

        stateMachine.ChangeState(player.idleState);
    }
}
