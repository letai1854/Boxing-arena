using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGroundState : AllyState
{
    public AllyGroundState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName) : base(Ally, AllyStateMachine, stateName)
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
        if (OpponentManager.Instance.EnemyTargets.Count == 0)
        {
            AllyStateMachine.ChangeState(Ally.AllyVictoryState);
        }
    }
}
