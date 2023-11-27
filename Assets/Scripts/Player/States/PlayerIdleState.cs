using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.isHoldingAttackButton)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.isBusy)
        {
            if(player.xInput != player.transform.right.x)
            {
                player.FaceTo(player.xInput);
            }
        }

        HandleGroundSlide();

        if (player.xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        if (!player.IsGrounded())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
