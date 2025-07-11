using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyKnockoutState : AllyState
{
    public AllyKnockoutState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName) : base(Ally, AllyStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Ally.capsuleCollider.enabled = false;

        Ally.boxCollider.enabled = true;

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
