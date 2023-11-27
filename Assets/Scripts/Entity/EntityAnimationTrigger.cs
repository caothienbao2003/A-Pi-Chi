using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    protected Entity entity => GetComponentInParent<Entity>();

    [SerializeField] protected LayerMask attackLayerMask;

    protected virtual void FinishAnimation()
    {
        entity.WhenFinishAnimation();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position,entity.attackCheckRadius, attackLayerMask);
        
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Entity>() != null)
            {
                Entity attackedEntity = hit.GetComponent<Entity>();
                Entity attackingEntity = GetComponentInParent<Entity>();
                
                attackedEntity.ChangeToHitState();
                attackingEntity.DealKnockbackTo(attackedEntity);
            }
        }
    }
}
