using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallHopState : PlayerAirState
{
    public PlayerWallHopState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.wallHopTime;

        rb.velocity = new Vector2(-player.transform.right.x * player.wallHopForce, player.wallHopForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0 || !player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
