using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInjureState : PlayerState
{
    public PlayerInjureState(Player player, PlayerStateMachine playerStateMachine, string stateName) : base(player, playerStateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.isAttackTarget = false;
    }
    public override void Update()
    {
        base.Update();
        if (player.isInjured)
        {

            playerStateMachine.ChangeState(player.playerIdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
