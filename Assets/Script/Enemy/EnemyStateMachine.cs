using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState currentState;

    public void Initialized(EnemyState enemyState)
    {
        currentState = enemyState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
}
