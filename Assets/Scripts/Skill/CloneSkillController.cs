using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private float cloneTimer;
    private float colorLosingSpeed;
    private CloneSkill cloneSkill;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        cloneSkill = SkillManager.instance.cloneSkill; 

        cloneTimer = cloneSkill.cloneDuration;
        colorLosingSpeed = cloneSkill.colorLosingSpeed;
    }

    void Update()
    {
        cloneTimer -= Time.deltaTime;
        if( cloneTimer <= 0 )
        {
            spriteRenderer.color = new Color(1,1,1,spriteRenderer.color.a - Time.deltaTime * colorLosingSpeed);
        }
    }
}
