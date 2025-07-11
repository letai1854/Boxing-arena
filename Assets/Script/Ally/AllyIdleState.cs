using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllyIdleState : AllyGroundState
{
    public AllyIdleState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName)
       : base(Ally, AllyStateMachine, stateName)
    {
    }

    Transform targetTransform = null;
    private Coroutine aiLogicCoroutine;
    bool hasMove = false;
    public override void Enter()
    {
        base.Enter();
        Ally.attackCount = Ally.attackCountTemp;
        if (Ally.attackCount > 1f)
        {
            Ally.attackCount = 0f;
        }
        Ally.isAttackTarget = false;
        Ally.isInjured = false;
        hasMove = false;
        aiLogicCoroutine = Ally.StartCoroutine(AI_Logic_Coroutine());
    }



    public override void Exit()
    {
        base.Exit();
        if (aiLogicCoroutine != null)
        {
            Ally.StopCoroutine(aiLogicCoroutine);
        }
    }

    public override void Update()
    {
        base.Update();

        if (Ally.currentTarget != null)
        {

        }

        if (Ally.IsMoving())
        {
            hasMove = true;
            AllyStateMachine.ChangeState(Ally.AllyWalkState);
        }
        else if (Ally.CheckAttacking() && !Ally.isAttackTarget && Ally.attackCooldownTimer < 0)
        {
            AllyStateMachine.ChangeState(Ally.AllyAttackState);
        }
        else if (Ally.IsPlayerNearby() && !Ally.IsMoving() && !Ally.CheckAttacking())
        {
            Transform targetToLookAt = OpponentManager.Instance.GetClosestTargetEnemy(Ally.transform.position);

            if (targetToLookAt != null)
            {
                Ally.LookAtTarget(targetToLookAt);
            }
        }


    }

    private IEnumerator AI_Logic_Coroutine()
    {
        float randomValue = Random.Range(0f, 0.5f);
        yield return new WaitForSeconds(randomValue);
        float stopThreshold = Ally.attackRange;
        float startThreshold = Ally.attackRange + 0.2f;
        while (true)
        {
            Vector3 AllyPosition = Ally.transform.position;
            targetTransform = OpponentManager.Instance.GetClosestTargetEnemy(AllyPosition);


            if (targetTransform != null)
            {

                float distanceToTarget = Vector3.Distance(AllyPosition, targetTransform.position);

                Ally.currentTarget = targetTransform;

                if (Ally.IsMoving())
                {
                    targetTransform.GetComponent<Entity>().RegisterAttacker(Ally);
                    if (distanceToTarget <= stopThreshold)
                    {
                        Ally.moveDirection = Vector3.zero;
                    }
                }
                else
                {
                    if (distanceToTarget > startThreshold)
                    {
                        Ally.moveDirection = (targetTransform.position - Ally.transform.position).normalized;
                    }
                }


            }
            else
            {
                //if (Ally.currentTarget != null)
                //{
                //    Ally.currentTarget.GetComponent<Entity>().UnregisterAttacker(Ally);
                //    Ally.currentTarget = null;
                //}
                //Ally.moveDirection = Vector3.zero;
                //Ally.currentTarget = null;
            }
            float randomWait = 1 / (GameManager.Instance.currentLevelIndex + 1);
            if (randomWait <= 0.25)
            {
                randomWait = 0.25f;
            }
            yield return new WaitForSeconds(randomWait);
        }
    }
}
