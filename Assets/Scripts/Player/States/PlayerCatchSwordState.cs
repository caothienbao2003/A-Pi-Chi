using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private ButtonManager buttonManager;
    public PlayerCatchSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        buttonManager = ButtonManager.instance;

        buttonManager.swordSkillButton.SetActiveArrowIcons(true);
        buttonManager.swordSkillButton.FreezeStick(false);
    }

    public override void WhenFinishAnimation()
    {
        base.WhenFinishAnimation();

        stateMachine.ChangeState(player.idleState);
    }
} 
