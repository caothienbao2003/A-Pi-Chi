using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected Skeleton skeleton;
    public SkeletonState(Skeleton skeleton, StateMachine stateMachine, string animBoolName) : base(skeleton, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }
}
