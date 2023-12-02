using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExistTimer;
    private CrystalAbility crystalAbility;
    private float moveSpeed;

    private bool canTeleport = false;
    private bool canGrow = false;
    private bool canExplode = false;
    private bool canMove = false;
    private bool canCreateClone = false;

    private float maxGrowSize = 3f;
    private float growSpeed = 5f;
    private Transform closestEnemy;

    //Explode
    private Vector2 explodeKnockbackDir;
    private float explodeKnockbackForce;

    //Create clone

    private Vector2 currentPos;
    private Player player;

    public void SetUpCrystal(float crystalDuration)
    {
        crystalExistTimer = crystalDuration;
        canTeleport = true;
    }

    public void SetUpExplode(Vector2 explodeKnockbackDir, float explodeKnockbackForce)
    {
        canExplode = true;

        this.explodeKnockbackDir = explodeKnockbackDir;
        this.explodeKnockbackForce = explodeKnockbackForce;
    }

    public void SetUpMirage()
    {
        canCreateClone = true;
    }

    public void SetUpMoveTowardEnemy(float moveSpeed, Transform closestEnemy)
    {
        canMove = true;
        canTeleport = false;

        this.moveSpeed = moveSpeed;
        this.closestEnemy = closestEnemy;
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxGrowSize, maxGrowSize), growSpeed * Time.deltaTime);
        }

        if (canMove)
        {
            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, closestEnemy.position) < 1f)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            canTeleport = false;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void AnimationExplodeTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius, GameManager.instance.enemyLayer);

        foreach (Collider2D hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.ChangeToHitState();
                Utilities.DealKnockback(transform.position, enemy, explodeKnockbackDir, explodeKnockbackForce);
            }
        }
    }

    public void Teleport(Transform playerTransform)
    {
        if (!canTeleport)
        {
            return;
        }

        if(canTeleport)
        {
            SkillManager.instance.cloneSkill.CreateClone(playerTransform, Vector2.zero);
        }

        Vector2 playerPos = playerTransform.position;

        playerTransform.position = transform.position;
        transform.position = playerPos;

        FinishCrystal();
    }

    private void GetCurrentPos()
    {
        currentPos = transform.position;
    }
}
