using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : SkeletonState
{
    private Collider2D player;
    private float normalSpeed;
    private float animationSpeedMultiplier = 2.5f;

    public SkeletonBattleState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player = skeleton.IsDetectingPlayer();
        normalSpeed = skeleton.anim.speed;
        skeleton.anim.speed = animationSpeedMultiplier;
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.anim.speed = normalSpeed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (!skeleton.IsDetectingPlayer())
        {
            stateMachine.ChangeState(skeleton.idleState);
        }
        else if(player != null)
        {
            bool isPlayerFront = (player.transform.position.x - skeleton.transform.position.x) * skeleton.transform.right.x > 0;
            if ((!skeleton.IsGrounded() || skeleton.IsTouchingWall()) && isPlayerFront)
            {
                stateMachine.ChangeState(skeleton.reactState);
            }
            else
            {
                if (Mathf.Abs(player.transform.position.x - skeleton.transform.position.x) < skeleton.attackDistance)
                {
                    if(stateMachine.currentState != skeleton.hitState)
                    {
                        stateMachine.ChangeState(skeleton.attackState);
                    }
                }
                else
                {
                    int faceDir = 0;

                    if (player.transform.position.x > skeleton.transform.position.x)
                    {
                        faceDir = 1;
                    }
                    else if (player.transform.position.x < skeleton.transform.position.x)
                    {
                        faceDir = -1;
                    }

                    skeleton.SetVelocity(faceDir * skeleton.battleMoveSpeed, 0);
                }
            }
        }
        

        

    }
}
