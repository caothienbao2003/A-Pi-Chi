using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuardState : PlayerState
{
    private ParrySkill parrySkill;
    public PlayerGuardState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackTimer;
        parrySkill = SkillManager.instance.parrySkill;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        HandleGroundSlide();
    }

    public override void Update()
    {
        base.Update();

        parrySkill.Guarding();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
