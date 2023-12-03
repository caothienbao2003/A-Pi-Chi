using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float moveDir = 0;

    public PlayerAirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gameInput.OnJumpRelease += GameInput_OnJumpRelease;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
    }

    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        player.OnSwordSkillPress();
    }

    private void GameInput_OnJumpRelease(object sender, System.EventArgs e)
    {
        player.coyoteTimeCounter = 0;
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * player.jumpHeightMultiplier);
    }

    public override void Exit()
    {
        base.Exit();
        gameInput.OnJumpRelease -= GameInput_OnJumpRelease;
        gameInput.OnSwordSkillPress -= GameInput_OnSwordSkillPress; 
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(new Vector2(player.movementForceInAir * player.xInput, 0), ForceMode2D.Force);

        if (player.xInput != 0)
        {
            moveDir = player.xInput;
        }

        if (player.xInput != 0)
        {
            if (Mathf.Abs(player.rb.velocity.x) > player.moveSpeed)
            {
                player.rb.velocity = new Vector2(moveDir * player.moveSpeed, player.rb.velocity.y);
            }
        }
        else
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x * player.airDragMultiplier, player.rb.velocity.y);
        }
    }

    public override void Update()
    {
        base.Update();
        player.FaceTo(player.xInput);

        if (player.IsTouchingWall() && !player.IsGrounded() && player.xInput != 0 && stateMachine.currentState != player.wallHopState)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(player.IsGrounded() && stateMachine.currentState != player.jumpState)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
