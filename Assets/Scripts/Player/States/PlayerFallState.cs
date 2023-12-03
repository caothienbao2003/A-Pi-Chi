using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gameInput.OnJumpPress += GameInput_OnJumpPress;
    }

    private void GameInput_OnJumpPress(object sender, System.EventArgs e)
    {
        if (player.coyoteTimeCounter > 0)
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        gameInput.OnJumpPress -= GameInput_OnJumpPress;
    }

    public override void Update()
    {
        base.Update();

        if(player.IsGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if(player.CanWallLedge())
        {
            stateMachine.ChangeState(player.wallLedgeState);
        }
        else if (player.IsTouchingWall() && !player.IsGrounded() && stateMachine.currentState != player.wallHopState)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
