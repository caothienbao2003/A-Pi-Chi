using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected Entity entity;
    protected StateMachine stateMachine;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public State(Entity entity, StateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        entity.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
