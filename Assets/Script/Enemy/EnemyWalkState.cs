using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyGroundState
{
    public EnemyWalkState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName)
        : base(enemy, enemyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.isAttackTarget = false;

        enemy.rb.constraints = enemy.rb.constraints | RigidbodyConstraints.FreezePositionY;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.rb.constraints = enemy.rb.constraints & ~RigidbodyConstraints.FreezePositionY;

    }

    public override void Update()
    {
        base.Update();

        //if (enemy.CheckAttacking())
        //{
        //    enemyStateMachine.ChangeState(enemy.EnemyIdleState);
        //}
        if (enemy.currentTarget != null)
        {



            float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.currentTarget.position);

            if (distanceToTarget > enemy.attackRange + 0.2f)
            {
                enemy.moveDirection = (enemy.currentTarget.position - enemy.transform.position).normalized;

            }
            else
            {
                enemy.moveDirection = Vector3.zero;
                enemy.currentTarget = null;
            }
        }
        if (!enemy.IsMoving())
        {
            enemyStateMachine.ChangeState(enemy.EnemyIdleState);

        }


        Vector3 forceDirection = enemy.moveDirection;
        forceDirection.y = 0;
        enemy.rb.AddForce(forceDirection * enemy.baseMoveSpeed, ForceMode.Force);
        if (enemy.moveDirection != Vector3.zero)
        {
            enemy.transform.rotation = Quaternion.LookRotation(enemy.moveDirection);
        }
    }
}
