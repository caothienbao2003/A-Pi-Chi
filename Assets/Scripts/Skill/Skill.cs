using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    public float coolDownTimer;
    protected Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(coolDownTimer <= 0)
        {
            return true;
        }
        
        return false;
    }

    public virtual void UseSkill()
    {
        
    }

    public void ResetCoolDownTimer()
    {
        coolDownTimer = coolDown;
    }

    protected Transform FindClosestEnemy(Transform startTransform,float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(startTransform.position, radius);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(startTransform.position, hit.transform.position);

                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
