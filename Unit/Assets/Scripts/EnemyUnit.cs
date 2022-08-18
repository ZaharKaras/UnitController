using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour
{
    private NavMeshAgent navAgent;

    public UnitStatType.Base baseStats;

    private Collider[] rangeColliders;

    private Transform aggroTarget;

    private bool hasAggro = false;

    private float distance;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(!hasAggro)
        {
            CheckForEnemyTargets();
        }
        if(hasAggro)
        {
            MoveToAggroTarget();
        }
    }

    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange);

        for(int i = 0; i < rangeColliders.Length; i++)
        {
            if(rangeColliders[i].gameObject.layer == UnitHandler.instance.pUnitLayer)
            {
                aggroTarget = rangeColliders[i].gameObject.transform;
                hasAggro = true;
            }
        }
    }

    private void MoveToAggroTarget()
    {
        distance = Vector3.Distance(aggroTarget.position, transform.position);
        navAgent.stoppingDistance = (baseStats.attackRange + 1);

        if(distance <= baseStats.aggroRange)
        {
            navAgent.SetDestination(aggroTarget.position);
        }
    }
}
