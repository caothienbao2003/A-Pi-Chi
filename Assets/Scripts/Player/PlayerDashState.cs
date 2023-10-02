using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashState : PlayerState
{
    private float dashDir;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        stateTimer = player.dashTime;

        dashDir = player.xInput;

        if(dashDir == 0)
        {
            dashDir = player.transform.right.x;
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
        player.DashCooldown();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(dashDir * player.dashSpeed, 0);

        if(!player.IsGrounded() && player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(stateTimer <= 0)
        {
            if (!player.IsGrounded())
            {
                stateMachine.ChangeState(player.fallState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
