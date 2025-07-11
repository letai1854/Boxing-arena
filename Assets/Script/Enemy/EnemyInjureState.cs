using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInjuredState : EnemyState
{
    public EnemyInjuredState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName)
        : base(enemy, enemyStateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.isAttackTarget = false;
    }
    public override void Update()
    {
        base.Update();
        if (enemy.isInjured)
        {

            enemyStateMachine.ChangeState(enemy.EnemyIdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}

