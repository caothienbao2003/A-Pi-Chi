using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;

    #region Reference
    private EntityEffect entityEffect => GetComponentInChildren<EntityEffect>();
    #endregion

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityStats stats { get; private set; }
    #endregion

    #region Parameter
    [Header("Collision")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Vector2 wallCheckSize;

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
        stats = GetComponent<EntityStats>();
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

    public virtual bool IsTouchingWall()
    {
        return Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, transform.right, 0, groundLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        //Draw Wall Check
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        //Draw Attack Check
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void FaceTo(float xValue)
    {
        if (xValue != 0)
        {
            xValue = xValue > 0 ? 1 : -1;
            transform.right = new Vector2(xValue, 0);
        }
    }
    #endregion

    #region Velocity
    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        FaceTo(x);
    }
    #endregion

    public virtual void WhenFinishAnimation()
    {
        stateMachine.currentState.WhenFinishAnimation();
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
    public virtual void DealKnockbackTo(Entity entity)
    {
        Vector2 knockBackDir = new Vector2(transform.right.x * currentKnockBackDir.x, currentKnockBackDir.y) * currentKnockBackForce;
        entity.rb.AddForce(knockBackDir, ForceMode2D.Impulse);
    }

    public virtual void DamageEffect()
    {

    }

    public virtual void ChangeToHitState()
    {

    }

    public virtual void ChangeCurrentStateTo(State state)
    {
        stateMachine.ChangeState(state);
    }

    public virtual void AddAForce(Vector2 moveDir, float force)
    {
        rb.AddForce(moveDir * force, ForceMode2D.Impulse);
    }
}
