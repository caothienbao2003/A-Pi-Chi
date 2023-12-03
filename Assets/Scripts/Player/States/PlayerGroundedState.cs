using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private ButtonManager buttonManager;
    public PlayerGroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        buttonManager = ButtonManager.instance;
        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnGuardPress += GameInput_OnGuardPress;
    }

    private void GameInput_OnGuardPress(object sender, System.EventArgs e)
    {
        player.OnGuardPress();
    }

    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        player.OnSwordSkillPress();
    }

    private void GameInput_OnJumpPress(object sender, System.EventArgs e)
    {
        stateMachine.ChangeState(player.jumpState);
    }

    public override void Exit()
    {
        base.Exit();
        gameInput.OnJumpPress-= GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress-= GameInput_OnSwordSkillPress;
        gameInput.OnGuardPress-= GameInput_OnGuardPress;
    }

    public override void Update()
    {
        base.Update();

        if(buttonManager.isHoldingAttackButton)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if(!player.IsGrounded())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
