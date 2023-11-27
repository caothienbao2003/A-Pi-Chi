using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerAnimationTrigger : EntityAnimationTrigger
{
    private void CounterAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckRadius, attackLayerMask);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                Player player = GetComponentInParent<Player>();

                enemy.ChangeToStunState();
                player.DealKnockbackTo(enemy);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword();
    }
}
