using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;

    #region Reference
    private EntityEffect entityEffect => GetComponentInChildren<EntityEffect>();
    #endregion

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region Parameter
    [Header("Collision")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    public Transform attackCheck;
    public float attackCheckRadius;

    #endregion

    #region Variables
    public Vector2 currentKnockBackDir { get; set; }
    public float currentKnockBackForce { get; set; }
    public bool canBeStunned { get; set; }
    #endregion

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    #region Collision
    public virtual bool IsGrounded() => Physics2D.CircleCast(groundCheck.position, .2f, Vector2.down, groundCheckDistance, groundLayer);

    public virtual bool IsTouchingWall() => Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        //Draw Wall Check
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //Draw Attack Check
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void HandleFlip(float moveX)
    {
        if (moveX != 0)
        {
            float moveDir = moveX;
            transform.right = new Vector2(moveDir, 0);
        }
    }
    #endregion

    #region Velocity
    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        HandleFlip(x);
    }
    #endregion

    public virtual void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public virtual void DealKnockbackTo(Entity entity)
    {
        Vector2 knockBackDir = new Vector2(transform.right.x * currentKnockBackDir.x, currentKnockBackDir.y) * currentKnockBackForce;
        entity.rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    protected virtual void ChangeAnimationWithDelay(string animBoolName, float time)
    {
        StartCoroutine(ChangeAnimationWithDelayCo(animBoolName, time));
    }

    private IEnumerator ChangeAnimationWithDelayCo(string animBoolName, float time)
    {
        anim.SetBool(animBoolName, false);
        yield return new WaitForSeconds(time);
        anim.SetBool(animBoolName, true);
    }

    public virtual void ChangeToHitState()
    {

    }
}
