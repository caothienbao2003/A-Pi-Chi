using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player player;
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        this.player = player;
    }
}
