using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : Singleton<OpponentManager>
{
    public  List<Transform> playerAndAllyTargets = new List<Transform>();

    public IReadOnlyList<Transform> PlayerAndAllyTargets => playerAndAllyTargets;


    public List<Transform> enemyTargets = new List<Transform>();

    public IReadOnlyList<Transform> EnemyTargets => enemyTargets;



    public void RegisterTargetEnemy(Transform target)
    {
        if (target != null && !enemyTargets.Contains(target))
        {
            enemyTargets.Add(target);
        }
    }

    public void UnregisterTargetEnemy(Transform target)
    {
        if (target != null && enemyTargets.Contains(target))
        {
            enemyTargets.Remove(target);
        }
    }

    public Transform GetClosestTargetEnemy(Vector3 position)
    {
        if (enemyTargets.Count == 0) return null;

        Transform closesttarget = null;
        float mindistancesqr = float.MaxValue;

        foreach (Transform target in enemyTargets)
        {
            float distancesqr = (position - target.position).sqrMagnitude;
            if (distancesqr < mindistancesqr)
            {
                mindistancesqr = distancesqr;
                closesttarget = target;
            }
        }
        return closesttarget;
    }





    public void RegisterTargetPlayer(Transform target)
    {
        if (target != null && !playerAndAllyTargets.Contains(target))
        {
            playerAndAllyTargets.Add(target);
        }
    }

    public void UnregisterTargetPlayer(Transform target)
    {
        if (target != null && playerAndAllyTargets.Contains(target))
        {
            playerAndAllyTargets.Remove(target);
        }
    }

    public Transform GetClosestTargetPlayer(Vector3 position)
    {
        if (playerAndAllyTargets.Count == 0) return null;

        Transform closesttarget = null;
        float mindistancesqr = float.MaxValue;

        foreach (Transform target in playerAndAllyTargets)
        {
            float distancesqr = (position - target.position).sqrMagnitude;
            if (distancesqr < mindistancesqr)
            {
                mindistancesqr = distancesqr;
                closesttarget = target;
            }
        }
        return closesttarget;
    }
    //public Transform GetClosestTarget(Vector3 position)
    //{
    //    if (playerAndAllyTargets.Count == 0) return null;

    //    Transform closestTarget = null;
    //    float minDistanceSqr = float.MaxValue;

    //    foreach (Transform target in playerAndAllyTargets)
    //    {
    //        Entity entity = target.GetComponent<Entity>();
    //        if (entity != null && entity.HasAttackSlotAvailable())
    //        {
    //            float distanceSqr = (position - target.position).sqrMagnitude;
    //            if (distanceSqr < minDistanceSqr)
    //            {
    //                minDistanceSqr = distanceSqr;
    //                closestTarget = target;
    //            }
    //        }
    //    }
    //    return closestTarget;
    //}

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
    }

   

    protected override void InitializeSingleton()
    {
        base.InitializeSingleton();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
