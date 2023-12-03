using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallLedgeState : PlayerState
{
    public PlayerWallLedgeState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        gameInput = GameInput.instance;
        gameInput.OnJumpPress += GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
    }

    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        player.OnSwordSkillRelease();
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

        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameInput.OnJumpPress -= GameInput_OnJumpPress;
        gameInput.OnSwordSkillPress -= GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease -= GameInput_OnSwordSkillRelease;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsTouchingWallLedge())
        {
            player.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        if (player.xInput != 0)
        {
            if (player.xInput * player.transform.right.x < 0)
            {
                stateMachine.ChangeState(player.wallHopState);
            }
        }
    }

}
