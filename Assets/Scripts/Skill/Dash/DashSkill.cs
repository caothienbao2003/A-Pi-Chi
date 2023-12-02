using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DashSkillType
{
    Basic,
    Clone,
    CloneTwice
}

public class DashSkill : Skill
{
    [SerializeField] private DashSkillType dashSkillType;
    private CloneSkill cloneSkill;

    private void Start()
    {
        cloneSkill = SkillManager.instance.cloneSkill;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        switch(dashSkillType)
        {
            case DashSkillType.Basic:
                break;
            case DashSkillType.Clone:
                cloneSkill.CreateClone(player.transform, Vector2.zero, true);
                break;
            case DashSkillType.CloneTwice:
                cloneSkill.CreateClone(player.transform, Vector2.zero, true);
                break;
        }
    }

    public void OnDashEnd()
    {
        if(dashSkillType == DashSkillType.CloneTwice)
        {
            cloneSkill.CreateClone(player.transform, Vector2.zero, true);
        }
    }
}
