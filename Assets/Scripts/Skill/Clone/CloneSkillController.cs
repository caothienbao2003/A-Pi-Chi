using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private float cloneTimer;
    private float colorLosingSpeed;
    private CloneSkill cloneSkill;

    private bool canAttack = false;
    private Player player;

    private int comboCounter;

    private Vector2 currentKnockbackDir;
    private float currentKnockbackForce;

    private float detectEnemyRadius;

    private Transform closestEnemy;

    [SerializeField] private Transform attackCheck;

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

        player = PlayerManager.instance.player;
        comboCounter = Random.Range(0, 3);

        currentKnockbackDir = player.knockBackAttackDir[comboCounter];
        currentKnockbackForce = player.knockBackAttackForce[comboCounter];

        detectEnemyRadius = cloneSkill.detectEnemyRadius;

        FaceClosestEnemy();

        if (canAttack)
        {
            anim.SetInteger("ComboCounter", comboCounter + 1);
        }
    }

    void Update()
    {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer <= 0)
        {
            spriteRenderer.color = new Color(1,1,1,spriteRenderer.color.a - Time.deltaTime * colorLosingSpeed);
        }

        if(spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FinishAnimation()
    {
        cloneTimer = 0;
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, player.attackCheckRadius, GameManager.instance.enemyLayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                enemy.ChangeToHitState();

                DealKnockbackTo(enemy);
            }
        }
    }

    private void DealKnockbackTo(Enemy enemy)
    {
        Vector2 knockBackDir = new Vector2(transform.right.x * currentKnockbackDir.x, currentKnockbackDir.y) * currentKnockbackForce;
        enemy.rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    private void FaceClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectEnemyRadius, GameManager.instance.enemyLayer);

        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if(closestEnemy != null)
        {
            canAttack = true;
            int facingX = closestEnemy.transform.position.x - transform.position.x > 0 ? 1 : -1;
            transform.right = new Vector2(facingX, 0);
        }   
        else
        {
            canAttack = false;
        }
    }
}
