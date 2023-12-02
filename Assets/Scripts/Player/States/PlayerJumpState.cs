using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(player.rb.velocity.y <0)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
