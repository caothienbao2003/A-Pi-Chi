using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitSwordState : PlayerState
{
    private SwordSkill swordSkill;
    public PlayerWaitSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        swordSkill = SkillManager.instance.swordSkill;

        swordSkill.ReturnAllSword();
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


        if(swordSkill.IsAllSwordReturned())
        {
            stateMachine.ChangeState(player.catchSwordState);
        }

    }
}
