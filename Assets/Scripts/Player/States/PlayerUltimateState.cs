using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUltimateState : PlayerState
{
    private UltimateSkill ultimateSkill;
    private float flyTimer;
    private float flySpeed;
    private float defaultGravityScale;
    private ButtonManager buttonManager;
    private SpriteRenderer playerSpriteRenderer;
    private float spriteFadeSpeed;
    private float alphaColor;
    private Color defaultColor;
    private Vector2 defaultLocalScale;
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

        buttonManager = ButtonManager.instance;
        buttonManager.HoldingUltimateButton();

        playerSpriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        defaultColor = playerSpriteRenderer.color;
        alphaColor = playerSpriteRenderer.color.a;
        defaultLocalScale = player.transform.localScale;    
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravityScale;
        ultimateSkill.isUsingSKill = false;

        buttonManager.EnableAll();
        ultimateSkill.ResetCoolDownTimer();
        playerSpriteRenderer.color = defaultColor;
        player.transform.localScale = defaultLocalScale;
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
            if (alphaColor >= 0)
            {
                alphaColor -= Time.fixedDeltaTime;
                player.transform.localScale = new Vector2(alphaColor, alphaColor); 
                playerSpriteRenderer.color = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, alphaColor);
            }
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
