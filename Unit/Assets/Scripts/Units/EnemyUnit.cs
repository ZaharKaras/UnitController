using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour
{
    private NavMeshAgent navAgent;
    public UnitStatType.Base stats;
    private Collider[] rangeColliders;
    private Transform aggroTarget;
    private PlayerUnit aggroUnit;
    private bool hasAggro = false;
    private float distance;
    public GameObject unitStatDisplay;
    public Image healthBarAmount;
    public float currentHealth;
    private float attackCooldown;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = stats.hp + stats.armor;
        navAgent.speed = stats.speed;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(!hasAggro)
        {
            CheckForEnemyTargets();
        }
        if(hasAggro)
        {
            MoveToAggroTarget();
        }
        
    }

    private void LateUpdate()
    {
        HandleHealth();
    }

    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, stats.aggroRange);

        for(int i = 0; i < rangeColliders.Length; i++)
        {
            if(rangeColliders[i].gameObject.layer == UnitHandler.instance.pUnitLayer)
            {
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponent<PlayerUnit>();
                hasAggro = true;
                break;
            }
        }
    }

    private void Attack()
    {
        if(attackCooldown <= 0 && distance <= stats.aggroRange + 1)
        {
            aggroUnit.TakeDamage(stats.damage);
            attackCooldown = stats.attackSpeed;
        }
    }

    
    private void MoveToAggroTarget()
    {
        if(aggroTarget == null)
        {
            navAgent.SetDestination(transform.position);
            hasAggro = false;
        }
        else
        {

            distance = Vector3.Distance(aggroTarget.position, transform.position);
            navAgent.stoppingDistance = (stats.attackRange + 1);

            if (distance <= stats.aggroRange)
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            if(distance <= stats.attackRange + 1)
            {
                navAgent.SetDestination(transform.position);
                Attack();
            }

        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void HandleHealth()
    {
        Camera camera = Camera.main;
        unitStatDisplay.transform.LookAt(unitStatDisplay.transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);

        healthBarAmount.fillAmount = currentHealth / (stats.hp + stats.armor);

        if (currentHealth <= 0)
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
