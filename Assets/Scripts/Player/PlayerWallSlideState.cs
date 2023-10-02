using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private float wallSlideHoldTime;
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        wallSlideHoldTime = player.wallSlideHoldTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.yInput < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(0, -player.wallSlideSpeed);
        }

        if (player.xInput != player.transform.right.x)
        {
            wallSlideHoldTime -= Time.deltaTime;
            if(wallSlideHoldTime <= 0)
            {
                wallSlideHoldTime = player.wallSlideHoldTime;
                stateMachine.ChangeState(player.wallHopState);
            }
        }
        else
        {
            wallSlideHoldTime = player.wallSlideHoldTime;
        }


        if (!player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
