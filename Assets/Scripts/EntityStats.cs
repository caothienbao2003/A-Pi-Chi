using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat maxHp;

    [SerializeField] private float currentHp;

    private Entity entity;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();

        currentHp = maxHp.GetValue();
    }

    public virtual void DealDamageTo(Entity entity)
    {
        float totalDamage = damage.GetValue() + strength.GetValue();

        entity.GetComponent<EntityStats>().TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHp -= _damage;

        if(currentHp <= 0)
        {
            Die();
            return;
        }

        entity.DamageEffect();
    }

    protected virtual void Die()
    {

    }
}
