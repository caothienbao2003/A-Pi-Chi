using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUltimateState : PlayerState
{
    private UltimateSkill ultimateSkill;
    private float flyTimer;
    private float flySpeed;
    private float defaultGravityScale;
    public PlayerUltimateState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        ultimateSkill = SkillManager.instance.ultimateSkill;

        ultimateSkill.isUsingSKill = true;

        flyTimer = ultimateSkill.flyTime;
        flySpeed = ultimateSkill.flySpeed;

        stateTimer = flyTimer + ultimateSkill.blackholeExistTime;

        defaultGravityScale = player.rb.gravityScale;

        player.rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravityScale;
        ultimateSkill.isUsingSKill = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (flyTimer > 0)
        {
            player.rb.velocity = Vector2.up * flySpeed;
        }
        else
        {
            player.rb.velocity = new Vector2(0, -.1f);
            ultimateSkill.UseSkill();

        }
    }

    public override void Update()
    {
        base.Update();

        flyTimer -= Time.deltaTime;

        if (ultimateSkill.HasExitBlackhole())
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
