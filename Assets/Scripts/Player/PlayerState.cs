using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player player;
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
    }

    protected void HandleGroundSlide()
    {
        if (player.IsGrounded())
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x * 0.85f, player.rb.velocity.y);
        }
    }
}
