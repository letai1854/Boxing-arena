using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyGroundState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName) 
        : base(enemy, enemyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy. attackCooldownTimer =enemy.attackCooldown;

        enemy.isAttackTarget = true;

    }

    public override void Exit()
    {
        base.Exit();
        enemy.attackCount += 0.5f;
        if (enemy.attackCount > 1f)
        {
            enemy.attackCount = 0f;
        }
        enemy.attackCountTemp = enemy.attackCount;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsMoving())
        {
            enemyStateMachine.ChangeState(enemy.EnemyWalkState);

        }
        else if (!enemy.isAttackTarget)
        {
            enemyStateMachine.ChangeState(enemy.EnemyIdleState);
        }
    }

}
