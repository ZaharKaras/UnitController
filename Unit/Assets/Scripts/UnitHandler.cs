using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler instance;

    [SerializeField]
    private Unit worker, warrior, healer;

    public LayerMask pUnitLayer, eUnitLayer;
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        eUnitLayer = LayerMask.NameToLayer("enemyUnits");
        pUnitLayer = LayerMask.NameToLayer("playerUnits");
    }

    public (float cost, float aggroRange,float damage, float attackRange, float hp, float armor) GetBasicUnitStats(string type)
    {
        Unit unit;
        switch (type)
        {
            case "worker":
                unit = worker;
                break;
            case "warrior":
                unit = warrior;
                break;
            case "healer":
                unit = healer;
                break;
            default:
                Debug.Log($"Unit Type:{type} could not be found or dows not exist");
                return (0, 0, 0, 0, 0, 0);
                break;
        }
        return (unit.baseStats.cost, unit.baseStats.aggroRange,unit.baseStats.damage, unit.baseStats.attackRange, unit.baseStats.hp, unit.baseStats.armor);
    }

    public void SetBasicUnitStats(Transform type)
    {
        Transform pUnits = PlayerManager.instance.playerUnits;
        Transform eUnits = PlayerManager.instance.enemyUnits;

        foreach(Transform child in type)
        {
            foreach(Transform unit in child)
            {
                string unitName = child.name.Substring(0, child.name.Length - 1).ToLower();
                var stats = GetBasicUnitStats(unitName);
                
                if (type == pUnits)
                {
                    PlayerUnit pU = unit.GetComponent<PlayerUnit>();
                    //set unit stats in each unit
                    pU.baseStats.cost = stats.cost;
                    pU.baseStats.aggroRange = stats.aggroRange;
                    pU.baseStats.damage = stats.damage;
                    pU.baseStats.attackRange = stats.attackRange;
                    pU.baseStats.hp = stats.hp;
                    pU.baseStats.armor = stats.armor;
                }
                else if(type == eUnits)
                {
                    //set enemy stats
                    EnemyUnit eU = unit.GetComponent<EnemyUnit>();
                    //set unit stats in each unit
                    eU.baseStats.cost = stats.cost;
                    eU.baseStats.aggroRange = stats.aggroRange;
                    eU.baseStats.damage = stats.damage;
                    eU.baseStats.attackRange = stats.attackRange;
                    eU.baseStats.hp = stats.hp;
                    eU.baseStats.armor = stats.armor;
                }
             

                //if we have any pugrades add them now
                //add upgrages to unit stats
            }
        }
    }
}
