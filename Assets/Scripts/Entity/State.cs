using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected Entity entity;
    protected StateMachine stateMachine;
    public string animBoolName { get; protected set; }

    protected float stateTimer;

    public State(Entity entity, StateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
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

    public virtual void WhenFinishAnimation()
    {

    }
}
