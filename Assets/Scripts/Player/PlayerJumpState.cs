using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y <0 && !player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
