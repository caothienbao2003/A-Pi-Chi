using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    private SwordSkill swordSkill;
    public PlayerAimSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        swordSkill = SkillManager.instance.swordSkill;

        swordSkill.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        swordSkill.DotsActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        HandleGroundSlide();
    }

    public override void Update()
    {
        base.Update();

        if(player.IsAimInputInCancelRange())
        {
            swordSkill.DotsActive(false);
        }
        else
        {
            swordSkill.SetupDotsPosition();
            swordSkill.DotsActive(true);
        }
    }
}
