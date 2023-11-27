using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
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
            coolDownTimer = coolDown;
            return true;
        }
        
        return false;
    }

    public virtual void UseSkill()
    {
        
    }

    public void ResetCoolDownTimer()
    {

    }
}
