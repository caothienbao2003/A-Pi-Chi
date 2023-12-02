using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParrySkillType
{
    Basic,
    CloneAttack,
    EnhanceStats
}

public class ParrySkill : Skill
{
    private CloneSkill cloneSkill;
    [SerializeField] private ParrySkillType parryType;

    [Header("Clone")]
    [SerializeField] private float cloneDelayTime = .5f;

    private bool canCreateClone = true;

    private void Start()
    {
        cloneSkill = SkillManager.instance.cloneSkill;
    }

    public void Guarding()
    {
        if (player.EnemiesDectected() != null)
        {
            foreach (var hit in player.EnemiesDectected())
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.canBeStunned)
                    {
                        player.ChangeCurrentStateTo(player.counterAttackState);

                        if (parryType == ParrySkillType.CloneAttack)
                        {
                            if(canCreateClone)
                            {
                                StartCoroutine(CreateCloneDelayCo(enemy));
                                canCreateClone = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator CreateCloneDelayCo(Enemy enemy)
    {
        yield return new WaitForSeconds(cloneDelayTime);
        cloneSkill.CreateClone(enemy.transform, new Vector2(2 * player.transform.right.x, 0));
        canCreateClone = true;
    }
}
