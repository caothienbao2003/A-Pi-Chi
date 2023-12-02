using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordState : PlayerState
{
    private SwordSkill swordSkill;
    private SwordSkillButton swordSkillButton;
    public PlayerThrowSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        swordSkill = SkillManager.instance.swordSkill;
        swordSkillButton = ButtonManager.instance.swordSkillButton;
    }

    public override void Exit()
    {
        base.Exit();

        if (!swordSkill.IsAllSwordThrown())
        {
            swordSkillButton.SetActiveArrowIcons(true);
        }
        else
        {
            swordSkillButton.FreezeStick(true);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void WhenFinishAnimation()
    {
        base.WhenFinishAnimation();

        stateMachine.ChangeState(player.idleState);
    }
}
