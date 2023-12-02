using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLosingSpeed = 1f;
    [Header("Detect Enemy")]
    [SerializeField] private float detectEnemyRadius;

    public void CreateClone(Transform spawnTransform, Vector3 offset)
    {
        Transform closestEnemy = FindClosestEnemy(spawnTransform, detectEnemyRadius);

        GameObject newClone = Instantiate(clonePrefab, spawnTransform.position + offset, Quaternion.identity);
        newClone.transform.right = spawnTransform.transform.right;

        CloneSkillController newCloneSkillController = newClone.GetComponent<CloneSkillController>();
        newCloneSkillController.SetUpClone(cloneDuration, colorLosingSpeed, detectEnemyRadius, closestEnemy, false);
        ResetCoolDownTimer();
    }

    public void CreateClone(Transform spawnTransform, Vector3 offset, bool isDash)
    {
        Transform closestEnemy = FindClosestEnemy(spawnTransform, detectEnemyRadius);

        GameObject newClone = Instantiate(clonePrefab, spawnTransform.position + offset, Quaternion.identity);
        newClone.transform.right = spawnTransform.transform.right;

        CloneSkillController newCloneSkillController = newClone.GetComponent<CloneSkillController>();
        newCloneSkillController.SetUpClone(cloneDuration, colorLosingSpeed, detectEnemyRadius, closestEnemy, isDash);
        ResetCoolDownTimer();
    }
}
