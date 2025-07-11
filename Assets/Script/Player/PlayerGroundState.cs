using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, string stateName) 
        : base(player, playerStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.isJump)
        {

        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(OpponentManager.Instance.EnemyTargets.Count == 0)
        {
            playerStateMachine.ChangeState(player.PlayerVictoryState);
        }

    }
}
