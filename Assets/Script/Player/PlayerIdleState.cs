using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string stateName)
        : base(player, playerStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isAttackTarget = false;
        player.isInjured = false;
        player.attackCount = player.attackCountTemp+0.5f;
        if (player.attackCount > 1f)
        {
            player.attackCount = 0f;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsMoving() && !player.isJump)
        {
            playerStateMachine.ChangeState(player.playerWalkState);
        }

        else if (player.CheckAttacking() && !player.isAttackTarget && player.attackCooldownTimer<0 && !player.isJump)
        {
            playerStateMachine.ChangeState(player.playerAttackState);
        }
    }

}
