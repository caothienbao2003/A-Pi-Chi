using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player player;
    protected GameInput gameInput;
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        gameInput = GameInput.instance;
    }

    protected void HandleGroundSlide()
    {
        if (player.IsGrounded())
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x * player.groundSlideMultiplier, player.rb.velocity.y);
        }
    }
}
