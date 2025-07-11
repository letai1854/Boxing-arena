using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public Enemy enemy;
    public EnemyStateMachine enemyStateMachine;
    public string stateName;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        enemy.anim.SetBool(stateName, true);
    }
    public virtual void Update()
    {

        enemy.anim.SetFloat("AttackCount", enemy.attackCount);

 
    }
    public virtual void Exit()
    {
        enemy.anim.SetBool(stateName, false);
    }
}
