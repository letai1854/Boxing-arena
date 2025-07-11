using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockoutState : PlayerState
{
    public PlayerKnockoutState(Player player, PlayerStateMachine playerStateMachine, string stateName) : base(player, playerStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player. capsuleCollider.enabled = false; 
        player.boxCollider.enabled =true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
