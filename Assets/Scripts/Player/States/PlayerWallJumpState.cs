using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.wallJumpTime;

        player.SetVelocity(-player.transform.right.x * player.wallJumpDir.x * player.wallJumpForce, player.wallJumpDir.y * player.wallJumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if(player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
