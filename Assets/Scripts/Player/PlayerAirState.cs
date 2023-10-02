using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float moveDir = 0;
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.AddForce(new Vector2(player.movementForceInAir * player.xInput, 0), ForceMode2D.Force);

        if (player.xInput != 0)
        {
            moveDir = player.xInput;
        }

        if (player.xInput != 0)
        {
            if (Mathf.Abs(rb.velocity.x) > player.moveSpeed)
            {
                rb.velocity = new Vector2(moveDir * player.moveSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * player.airDragMultiplier, rb.velocity.y);
        }
    }

    public override void Update()
    {
        base.Update();
        player.HandleFlip(player.xInput);

        if (player.IsTouchingWall() && stateMachine.currentState != player.wallHopState)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(player.IsGrounded() && stateMachine.currentState != player.jumpState)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
