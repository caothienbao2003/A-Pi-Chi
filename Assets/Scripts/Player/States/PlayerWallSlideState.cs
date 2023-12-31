using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private float wallSlideHoldTime;

    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        wallSlideHoldTime = player.wallSlideHoldTime;
        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
    }

    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        player.OnSwordSkillRelease();
    }

    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        player.OnSwordSkillPress();
    }

    private void GameInput_OnJumpPress(object sender, System.EventArgs e)
    {
        stateMachine.ChangeState(player.wallJumpState);
    }

    public override void Exit()
    {
        base.Exit();
        gameInput.OnJumpPress -= GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress -= GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease -= GameInput_OnSwordSkillRelease;
    }

    public override void Update()
    {
        base.Update();

        if(player.CanWallLedge())
        {
            stateMachine.ChangeState(player.wallLedgeState);
        }

        if (player.yInput < 0)
        {
            player.SetVelocity(0, player.rb.velocity.y);
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
