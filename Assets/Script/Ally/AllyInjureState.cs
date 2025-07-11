using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyInjuredState : AllyState
{
    public AllyInjuredState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName)
        : base(Ally, AllyStateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Ally.isAttackTarget = false;
    }
    public override void Update()
    {
        base.Update();
        if (Ally.isInjured)
        {

            AllyStateMachine.ChangeState(Ally.AllyIdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}

