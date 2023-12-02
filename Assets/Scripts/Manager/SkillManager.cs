using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public ParrySkill parrySkill { get; private set; }
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }
    public SwordSkill swordSkill { get; private set; }   
    public UltimateSkill ultimateSkill { get; private set; }
    public CrystalSkill crystalSkill { get; private set; }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        parrySkill = GetComponent<ParrySkill>();
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        ultimateSkill = GetComponent<UltimateSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }

    private void Start()
    {

    }
}
