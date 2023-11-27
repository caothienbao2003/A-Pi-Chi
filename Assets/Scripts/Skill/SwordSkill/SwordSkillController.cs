using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private SwordType swordType;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D thisCollider;
    private Player player;
    private float returnSpeed;
    private float dontHitTime;
    private float spawnTimer;
    private float catchSwordForce;

    private bool canRotate = true;
    private bool isReturning = false;
    private bool isCollided = false;

    //Bounce info
    private int amountOfBounce;
    private float bounceSpeed;
    private float bounceRadius;

    //Pierce info
    private int pierceAmount;

    //Spin info
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isSpining;
    private float spinHitTimer;
    private float spinHitCooldown;

    private List<Transform> enemyTarget;
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
        enemyTarget = new List<Transform>();
    }

    private void Start()
    {
        if (swordType == SwordType.Bounce || swordType == SwordType.Spin)
        {
            anim.SetBool("Spin", true);
        }
    }

    public void SetUpSword(Vector2 direction, float throwForce, float gravityScale, float returnSpeed, Player player, float dontHitTime, float catchSwordFloat, SwordType swordType)
    {
        rb.velocity = direction * throwForce;
        rb.gravityScale = gravityScale;

        this.player = player;
        this.returnSpeed = returnSpeed;
        this.dontHitTime = dontHitTime;
        this.catchSwordForce = catchSwordFloat;

        this.swordType = swordType;
    }

    public void SetUpBounce(int amountOfBounce, float bounceSpeed, float bounceRadius)
    {
        this.amountOfBounce = amountOfBounce;
        this.bounceSpeed = bounceSpeed;
        this.bounceRadius = bounceRadius;
    }

    public void SetUpPierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetUpSpin(float maxTravelDistance, float spinDuration, float spinHitCooldown)
    {
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.spinHitCooldown = spinHitCooldown;

        spinTimer = spinDuration;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (Vector2.Distance(player.transform.position, transform.position) > 30f)
        {
            ReturnSword();
        }

        switch (swordType)
        {
            case SwordType.Bounce:
                HandleBounce();
                break;
            case SwordType.Spin:
                HandleSpin();
                break;
        }
    }

    private void FixedUpdate()
    {
        //If the sword return is trigger or if after a while the sword didnt touch anything then move it back to player
        if (isReturning || (spawnTimer >= dontHitTime && !isCollided))
        {
            SwordMoveBackToPlayer();
        }
    }

    private void HandleBounce()
    {
        if (enemyTarget.Count > 0 && !isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    ReturnSword();
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void HandleSpin()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= maxTravelDistance && !isSpining)
        {
            isSpining = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        if (isSpining)
        {
            spinTimer -= Time.deltaTime;

            if (spinTimer < 0)
            {
                ReturnSword();
            }
        }

        spinHitTimer -= Time.deltaTime;

        if (spinHitTimer <= 0)
        {
            spinHitTimer = spinHitCooldown;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f, GameManager.instance.enemyLayer);

            foreach (Collider2D hit in colliders)
            {
                if (hit.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    DealKnockbackTo(enemy);
                }
            }
        }
    }

    public void ReturnSword()
    {
        canRotate = true;
        thisCollider.enabled = false;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;

        transform.parent = null;
        isReturning = true;

        Utilities.IgnoreLayerCollisionBetween(GameManager.instance.groundLayer, GameManager.instance.swordLayer, false);
    }

    private void SwordMoveBackToPlayer()
    {
        Vector2 moveDir = (player.transform.position - transform.position);
        rb.velocity = moveDir.normalized * returnSpeed;

        if (Vector2.Distance(transform.position, player.transform.position) < 0.7f)
        {
            player.rb.AddForce(catchSwordForce * moveDir, ForceMode2D.Impulse);
            player.FaceTo(transform.position.x - player.transform.position.x);
            player.CatchSword();

            DestroyThisSword();
        }

        anim.SetBool("Spin", true);
    }

    private void DestroyThisSword()
    {
        SkillManager.instance.swordSkill.DestroySword(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            DealKnockbackTo(enemy);
        }

        isCollided = true;

        switch (swordType)
        {
            case SwordType.Regular:
                RegularSwordTouch(collision);
                break;
            case SwordType.Bounce:
                BounceSwordTouch(collision);
                break;
            case SwordType.Pierce:
                PierceSwordTouch(collision);
                break;
            case SwordType.Spin:
                SpinSwordTouch(collision);
                break;
        }
    }

    private void DealKnockbackTo(Enemy enemy)
    {
        Vector2 attackDir = (enemy.transform.position - transform.position).normalized;

        if (swordType == SwordType.Spin)
        {
            attackDir.x = -attackDir.x;
        }

        attackDir.y = 1;

        enemy.rb.velocity = attackDir * 5f;
        enemy.ChangeToHitState();
    }

    private void BounceSwordTouch(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            Utilities.IgnoreLayerCollisionBetween(GameManager.instance.groundLayer, GameManager.instance.swordLayer, true);

            if (collision.GetComponent<Enemy>() != null)
            {
                if (enemyTarget.Count <= 0)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceRadius, GameManager.instance.enemyLayer);

                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                        {
                            enemyTarget.Add(collider.transform);
                        }
                    }
                }
            }

            anim.SetBool("Spin", true);
            transform.parent = null;
        }
        else
        {
            RegularSwordTouch(collision);

        }
    }

    private void PierceSwordTouch(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
        }
        else
        {
            RegularSwordTouch(collision);
        }
    }

    private void SpinSwordTouch(Collider2D collision)
    {
        isSpining = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void RegularSwordTouch(Collider2D collision)
    {
        canRotate = false;
        thisCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
        anim.SetBool("Spin", false);
    }
}
