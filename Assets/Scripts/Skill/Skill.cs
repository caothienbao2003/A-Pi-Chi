using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;

    private void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(coolDownTimer <= 0)
        {
            coolDownTimer = coolDown;
            return true;
        }
        
        return false;
    }

    public virtual void UseSkill()
    {

    }
}
