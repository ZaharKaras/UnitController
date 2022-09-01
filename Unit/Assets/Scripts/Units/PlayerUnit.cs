using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;

    private NavMeshAgent navAgent;
    public UnitStatType.Base stats;
    public GameObject unitStatDisplay;
    public Image healthBarAmount;
    public float currentHealth;
    private float attackCooldown;
    private Collider[] rangeColliders;
    public Transform aggroTarget;
    public EnemyUnit aggroUnit;
    public bool hasAggro = false;
    private float distance;
    private Vector3 newPosition;
    public bool isControlled;

    public int minerals;
    public Transform storage;
    public Transform mine;
    public bool isGathered;


    public void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = stats.hp + stats.armor;
        newPosition = transform.position;
        navAgent.speed = stats.speed;
        isGathered = false;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Vector3.Distance(newPosition, transform.position) <= 1.5 || isControlled)
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
    }

    private void FixedUpdate()
    {
        if(stats.name == "Worker")
        {
            if (isGathered)
            {
                GatherMinerals();
            }
        }
    }

    private void LateUpdate()
    {
        HandleHealth();

        if (aggroUnit == null)
            isControlled = false;

    }

    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, stats.aggroRange);

        for (int i = 0; i < rangeColliders.Length; i++)
        {
            if (rangeColliders[i].gameObject.layer == UnitHandler.instance.eUnitLayer)
            {
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroUnit = aggroTarget.gameObject.GetComponent<EnemyUnit>();
                hasAggro = true;
                isGathered = false;
                break;
            }
        }
    }

    private void Attack()
    {
        if (attackCooldown <= 0 && distance <= stats.aggroRange + 1)
        {
            if(stats.name == "Archer")
            {
                var position = transform.position + transform.forward;
                position.y = 0.5f;
                position.z = 1;
                var rotation = transform.rotation;
                var projectile = Instantiate(projectilePrefab, position, rotation);
                projectile.Fire(stats.attackSpeed, transform.forward);
            }
            else
            {
                aggroUnit.TakeDamage(stats.damage);
                attackCooldown = stats.attackSpeed;
            }
        }
    }

    public void MoveToAttack(Transform _aggroTarget)
    {
        newPosition = _aggroTarget.position;
        aggroTarget = _aggroTarget;
        aggroUnit = aggroTarget.gameObject.GetComponent<EnemyUnit>();
        hasAggro = true;
        isGathered=false;

        MoveToAggroTarget();
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
            navAgent.stoppingDistance = (stats.attackRange + 1);
            //Debug.Log("Distance: " + distance);

            if (distance <= stats.aggroRange || isControlled)
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            if (distance <= stats.attackRange + 1)
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
        isGathered = false;
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

    private void GatherMinerals()
    {
        

        if(minerals > 0)
        {
            if (storage == null)
                FindNearStorage();

            Invoke("DeliverMinerals", 2f);
        }
        else
        {
            if (mine == null)
            {
                isGathered = false;
                return;
            }

            if (Vector3.Distance(mine.position, transform.position) <= 2)
            {
                Debug.Log("gather");

                navAgent.SetDestination(transform.position);
                Mine _mine = mine.gameObject.GetComponent<Mine>();
                minerals = _mine.GetMinerals();
            }
            else
            {
                navAgent.SetDestination(mine.position);
            }
        }

    }

    private void FindNearStorage()
    {
        GameObject[] storages = GameObject.FindGameObjectsWithTag("Storage");
        List<float> distances = new List<float>();

        foreach(var stor in storages)
        {
            Transform storTransform = stor.transform;
            distances.Add(Vector3.Distance(storTransform.position, transform.position));
        }

        int minIndex = 0;
        for(int i = 0; i < distances.Count; i++)
        {
            if(distances[minIndex] >= distances[i])
                minIndex = i;
        }

        storage = storages[minIndex].transform;
    }

    private void DeliverMinerals()
    {
        if(minerals == 0)
        {
            GatherMinerals();
        }
        else
        {
            if (Vector3.Distance(transform.position, storage.position) <= 2)
            {
                Debug.Log("delivery");

                navAgent.SetDestination(transform.position);
                Building _storage = storage.gameObject.GetComponent<Building>();
                _storage.TakeMinerals(minerals);
                minerals = 0;
            }
            else
            {
                navAgent.SetDestination(storage.position);
            }
        }

    }
}
