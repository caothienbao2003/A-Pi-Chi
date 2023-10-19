using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    public float cloneDuration;
    public float colorLosingSpeed;

    public void CreateClone(Transform spawnTransform)
    {
        GameObject newClone = Instantiate(clonePrefab, spawnTransform.position, Quaternion.identity);
        newClone.transform.right = spawnTransform.right;
    }
}
