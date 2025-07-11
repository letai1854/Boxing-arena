using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyWalkState : AllyGroundState
{
    public AllyWalkState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName)
        : base(Ally, AllyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Ally.isAttackTarget = false;

        Ally.rb.constraints = Ally.rb.constraints | RigidbodyConstraints.FreezePositionY;
    }

    public override void Exit()
    {
        base.Exit();
        Ally.rb.constraints = Ally.rb.constraints & ~RigidbodyConstraints.FreezePositionY;

    }

    public override void Update()
    {
        base.Update();

        //if (Ally.CheckAttacking())
        //{
        //    AllyStateMachine.ChangeState(Ally.AllyIdleState);
        //}
        if (Ally.currentTarget != null)
        {



            float distanceToTarget = Vector3.Distance(Ally.transform.position, Ally.currentTarget.position);

            if (distanceToTarget > Ally.attackRange + 0.2f)
            {
                Ally.moveDirection = (Ally.currentTarget.position - Ally.transform.position).normalized;

            }
            else
            {
                Ally.moveDirection = Vector3.zero;
                Ally.currentTarget = null;
            }
        }
        if (!Ally.IsMoving())
        {
            AllyStateMachine.ChangeState(Ally.AllyIdleState);

        }


        Vector3 forceDirection = Ally.moveDirection;
        forceDirection.y = 0;
        Ally.rb.AddForce(forceDirection * Ally.baseMoveSpeed, ForceMode.Force);
        if (Ally.moveDirection != Vector3.zero)
        {
            Ally.transform.rotation = Quaternion.LookRotation(Ally.moveDirection);
        }
    }
}
