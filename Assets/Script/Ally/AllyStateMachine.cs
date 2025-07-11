using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStateMachine : MonoBehaviour
{
    public AllyState currentState;

    public void Initialized(AllyState AllyState)
    {
        currentState = AllyState;
        currentState.Enter();
    }

    public void ChangeState(AllyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
}
