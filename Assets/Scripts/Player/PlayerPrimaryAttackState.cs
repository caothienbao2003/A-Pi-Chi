using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float maxCombo = 2;
    private float comboTime;

    public PlayerPrimaryAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;
        comboTime = player.comboTime;        


        if(comboCounter > maxCombo || Time.time >= lastTimeAttacked + comboTime)
        {
            comboCounter = 0;
        }

        player.currentKnockBackDir = player.knockBackAttackDir[comboCounter];
        player.currentKnockBackForce = player.knockBackAttackForce[comboCounter];

        player.anim.SetInteger("ComboCounter", comboCounter);

        player.SetVelocity(player.attackMovements[comboCounter].x * player.transform.right.x, player.attackMovements[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            player.SetVelocity(0, player.rb.velocity.y);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
