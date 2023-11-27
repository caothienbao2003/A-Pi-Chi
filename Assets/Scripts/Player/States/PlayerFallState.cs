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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(player.IsGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsTouchingWall() && !player.IsGrounded() && stateMachine.currentState != player.wallHopState)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
