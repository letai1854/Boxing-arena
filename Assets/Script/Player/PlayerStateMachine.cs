using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState;

    public void Initialized(PlayerState playerState)
    {
        currentState = playerState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        
        currentState = newState;
        currentState.Enter();
    }
}
