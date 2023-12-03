using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerAnimationTrigger : EntityAnimationTrigger
{
    protected override void AttackTrigger()
    {
        base.AttackTrigger();
    }

    private void CounterAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckRadius, attackLayerMask);

        Player player = GetComponentInParent<Player>();
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                
                enemy.ChangeToStunState();
                player.stats.DealDamageTo(enemy);
            
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
