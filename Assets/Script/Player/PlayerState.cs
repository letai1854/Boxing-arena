using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    public Player player;
    public PlayerStateMachine playerStateMachine;
    public string stateName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string stateName)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(stateName, true);
    }   
    public virtual void Update()
    {

        player.anim.SetFloat("AttackCount", player.attackCount);
    }
    public virtual void Exit()
    {
        player.anim.SetBool(stateName, false);
    }
}
