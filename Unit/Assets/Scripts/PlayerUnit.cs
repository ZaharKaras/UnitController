using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : MonoBehaviour
{
    private NavMeshAgent navAgent;

    public UnitStatType.Base baseStats;

    public GameObject unitStatDisplay;

    public Image healthBarAmount;

    public float currentHealth;

    private float attackCooldown;

    private Collider[] rangeColliders;

    private Transform aggroTarget;

    private EnemyUnit aggroUnit;

    private bool hasAggro = false;

    private float distance;

    private Vector3 newPosition;
    

    public void OnEnable()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        currentHealth = baseStats.hp + baseStats.armor;
        newPosition = transform.position;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Vector3.Distance(newPosition, transform.position) <= 1.5)
        {

            if (!hasAggro)
            {
                CheckForEnemyTargets();
            }
            else
            {
                MoveToAggroTarget();
            }
        }

        Debug.Log(Vector3.Distance(newPosition, transform.position));
    }

    private void LateUpdate()
    {
        HandleHealth();
    }

    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange);

        for (int i = 0; i < rangeColliders.Length; i++)
        {
            if (rangeColliders[i].gameObject.layer == UnitHandler.instance.eUnitLayer)
            {
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponent<EnemyUnit>();
                hasAggro = true;
                break;
            }
        }
    }

    private void Attack()
    {
        if (attackCooldown <= 0 && distance <= baseStats.aggroRange + 1)
        {
            aggroUnit.TakeDamage(baseStats.damage);
            attackCooldown = baseStats.attackSpeed;
        }
    }

    private void MoveToAggroTarget()
    {
        if (aggroTarget == null)
        {
            navAgent.SetDestination(transform.position);
            hasAggro = false;
        }
        else
        {

            distance = Vector3.Distance(aggroTarget.position, transform.position);
            navAgent.stoppingDistance = (baseStats.attackRange + 1);

            if (distance <= baseStats.aggroRange)
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            if (distance <= baseStats.attackRange + 1)
            {
                navAgent.SetDestination(transform.position);
                Attack();
            }

        }
    }
    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
        newPosition = _destination;
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = (damage);
        currentHealth -= totalDamage;
    }


    private void HandleHealth()
    {
        Camera camera = Camera.main;
        unitStatDisplay.transform.LookAt(unitStatDisplay.transform.position + camera.transform.rotation * Vector3.forward, 
            camera.transform.rotation * Vector3.up);

        healthBarAmount.fillAmount = currentHealth / (baseStats.hp + baseStats.armor);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        InputHandler.instance.selectedUnits.Remove(gameObject.transform);
        Destroy(gameObject);
    }
}
