using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerGroundState
{
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, string stateName) : base(player, playerStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttackTarget = true;
   
    }

    public override void Exit()
    {
        base.Exit();
        player.attackCount += 0.5f;
        if (player.attackCount > 1f)
        {
            player.attackCount = 0f;
        }
        player.attackCountTemp = player.attackCount;
    }

    public override void Update()
    {
        base.Update();
        if (player.IsMoving()&& !player.isJump)
        {
            playerStateMachine.ChangeState(player.playerWalkState);

        }
        else if (!player.isAttackTarget && !player.isJump)
        {
            playerStateMachine.ChangeState(player.playerIdleState);
        }
    }

    

}
