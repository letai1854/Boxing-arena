using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine, string stateName) 
        : base(player, playerStateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.constraints = player.rb.constraints | RigidbodyConstraints.FreezePositionY;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.constraints = player.rb.constraints & ~RigidbodyConstraints.FreezePositionY;

    }

    public override void Update()
    {
        base.Update();

        if (!player.IsMoving())
        {
            playerStateMachine.ChangeState(player.playerIdleState);

        }

        Vector3 forceDirection = player.moveDirection;
        forceDirection.y = 0;
        player.rb.AddForce(forceDirection * player.baseMoveSpeed, ForceMode.Force);
        if (player.moveDirection != Vector3.zero)
        {
            player.transform.rotation = Quaternion.LookRotation(player.moveDirection);
        }
    }
    
}
