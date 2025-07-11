using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAttackState : AllyGroundState
{
    public AllyAttackState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName)
        : base(Ally, AllyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Ally.isAttackTarget = true;
        Ally.attackCooldownTimer = Ally.attackCooldown;

    }

    public override void Exit()
    {
        base.Exit();
        Ally.attackCount += 0.5f;
        if (Ally.attackCount > 1f)
        {
            Ally.attackCount = 0f;
        }
        Ally.attackCountTemp = Ally.attackCount;
    }

    public override void Update()
    {
        base.Update();
        if (Ally.IsMoving())
        {
            AllyStateMachine.ChangeState(Ally.AllyWalkState);

        }
        else if (!Ally.isAttackTarget)
        {
            AllyStateMachine.ChangeState(Ally.AllyIdleState);
        }
    }

}
