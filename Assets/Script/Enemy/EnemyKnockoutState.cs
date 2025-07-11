using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockoutState : EnemyState
{
    public EnemyKnockoutState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName) : base(enemy, enemyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.capsuleCollider.enabled = false;

        enemy.boxCollider.enabled = true;

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
