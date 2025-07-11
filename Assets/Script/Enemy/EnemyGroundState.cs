using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundState : EnemyState
{
    public EnemyGroundState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName) : base(enemy, enemyStateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (OpponentManager.Instance.PlayerAndAllyTargets.Count == 0)
        {
            enemyStateMachine.ChangeState(enemy.EnemyVictoryState);
        }
    }
}
