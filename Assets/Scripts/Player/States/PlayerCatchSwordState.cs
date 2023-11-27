using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void WhenFinishAnimation()
    {
        base.WhenFinishAnimation();

        stateMachine.ChangeState(player.idleState);
    }
} 
