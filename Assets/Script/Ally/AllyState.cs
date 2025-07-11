using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyState
{
    public Ally Ally;
    public AllyStateMachine AllyStateMachine;
    public string stateName;

    public AllyState(Ally Ally, AllyStateMachine AllyStateMachine, string stateName)
    {
        this.Ally = Ally;
        this.AllyStateMachine = AllyStateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        Ally.anim.SetBool(stateName, true);
    }
    public virtual void Update()
    {

        Ally.anim.SetFloat("AttackCount", Ally.attackCount);
    
    }
    public virtual void Exit()
    {
        Ally.anim.SetBool(stateName, false);
    }
}
