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

    public void OnEnable()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        currentHealth = baseStats.hp + baseStats.armor;
    }

    private void Update()
    {
        HandleHealth();
    }

    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
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
