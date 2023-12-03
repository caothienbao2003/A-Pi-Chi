using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    private SwordSkill swordSkill;
    private ButtonManager buttonManager;
    public PlayerAimSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        swordSkill = SkillManager.instance.swordSkill;

        swordSkill.DotsActive(true);
        buttonManager = ButtonManager.instance;

        buttonManager.HoldingSwordSkillButton();
        buttonManager.swordSkillButton.SetActiveArrowIcons(false);

        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
    }

    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        player.OnSwordSkillRelease();
    }

    public override void Exit()
    {
        base.Exit();

        swordSkill.DotsActive(false);
        buttonManager.EnableAll();

        gameInput.OnSwordSkillRelease -= GameInput_OnSwordSkillRelease;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        HandleGroundSlide();
    }

    public override void Update()
    {
        base.Update();

        player.FaceTo(player.aimSwordDirectionInput.x);

        if (player.IsAimInputInCancelRange())
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
