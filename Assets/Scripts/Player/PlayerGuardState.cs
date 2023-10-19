using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuardState : PlayerState
{
    public PlayerGuardState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.velocity = Vector2.zero;
        stateTimer = player.counterAttackTimer;
    }

    public override void Exit()
    {
        base.Exit();

        player.GuardCoolDown();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(player.EnemiesDectected() != null)
        {
            foreach (var hit in player.EnemiesDectected())
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if(enemy.canBeStunned)
                    {
                        stateMachine.ChangeState(player.counterAttackState);
                    }
                }
            }
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
