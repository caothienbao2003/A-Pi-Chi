using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();

    [SerializeField] private LayerMask attackLayerMask;

    private void AnimationTrigger()
    {
        entity.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position,entity.attackCheckRadius, attackLayerMask);
        
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Entity>() != null)
            {
                hit.GetComponent<Entity>().Damage();
            }
        }
    }
}
