using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }
}
