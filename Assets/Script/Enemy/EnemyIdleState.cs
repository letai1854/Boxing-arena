using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyIdleState : EnemyGroundState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine, string stateName)
       : base(enemy, enemyStateMachine, stateName)
    {
    }

    Transform targetTransform = null;
    private Coroutine aiLogicCoroutine;
    bool hasMove = false;
    public override void Enter()
    {
        base.Enter();
        enemy. attackCount = enemy.attackCountTemp;
        if (enemy.attackCount > 1f)
        {
            enemy.attackCount = 0f;
        }
        enemy.isAttackTarget = false;
        enemy.isInjured = false;
        hasMove = false;
        aiLogicCoroutine = enemy.StartCoroutine(AI_Logic_Coroutine());
    }



    public override void Exit()
    {
        base.Exit();
        if (aiLogicCoroutine != null)
        {
            enemy.StopCoroutine(aiLogicCoroutine);
        }
    }

    public override void Update()
    {
        base.Update();


        if (enemy.IsMoving())
        {
            hasMove = true;
            enemyStateMachine.ChangeState(enemy.EnemyWalkState);
        }
        else if (enemy.CheckAttacking() && !enemy.isAttackTarget && enemy.attackCooldownTimer < 0)
        {
            enemyStateMachine.ChangeState(enemy.EnemyAttackState);
        }
        else if (enemy.IsPlayerNearby() && !enemy.IsMoving() &&!enemy.CheckAttacking())
        {
            Transform targetToLookAt = OpponentManager.Instance.GetClosestTargetPlayer(enemy.transform.position);

            if (targetToLookAt != null)
            {
                enemy.LookAtTarget(targetToLookAt);
            }
        }
      

    }

    private IEnumerator AI_Logic_Coroutine()
    {

        
        float randomValue = Random.Range(0f, 0.5f);   
        yield return new WaitForSeconds(randomValue);
        float stopThreshold = enemy.attackRange;
        float startThreshold = enemy.attackRange + 0.2f;
        while (true)
        {
            Vector3 enemyPosition = enemy.transform.position;
            targetTransform = OpponentManager.Instance.GetClosestTargetPlayer(enemyPosition);
         

            if (targetTransform!=null)
            {

                float distanceToTarget = Vector3.Distance(enemyPosition, targetTransform.position);
                
                enemy.currentTarget = targetTransform;

                if (enemy.IsMoving())
                {
                targetTransform.GetComponent<Entity>().RegisterAttacker(enemy);
                    if (distanceToTarget <= stopThreshold)
                    {
                        enemy.moveDirection = Vector3.zero;
                    }
                }
                else
                {
                    if (distanceToTarget > startThreshold)
                    {
                        enemy.moveDirection = (targetTransform.position - enemy.transform.position).normalized;
                    }
                }


            }
            else
            {
                //if (enemy.currentTarget != null)
                //{
                //    enemy.currentTarget.GetComponent<Entity>().UnregisterAttacker(enemy);
                //    enemy.currentTarget = null;
                //}
                //enemy.moveDirection = Vector3.zero;
                //enemy.currentTarget = null;
            }
            float randomWait = 1 / (GameManager.Instance.currentLevelIndex + 1);
            if (randomWait<=0.25)
            {
                randomWait = 0.25f;
            }
            yield return new WaitForSeconds(randomWait);
        }
    }   
}
