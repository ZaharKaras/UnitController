using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler instance;

    [SerializeField]
    private UnitStats worker, warrior, healer;

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

    public UnitStatType.Base GetBasicUnitStats(string type)
    {
        UnitStats unit;
        switch (type)
        {
            case "worker":
                unit = worker;
                break;
            case "warrior":
                unit = warrior;
                break;
            case "archer":
                unit = healer;
                break;
            default:
                Debug.Log($"UnitStats Type:{type} could not be found or dows not exist");
                return null;
        }

        return unit.baseStats; //return whole class
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
                
                if (type == pUnits)
                {
                    PlayerUnit pU = unit.GetComponent<PlayerUnit>();
                    pU.stats = GetBasicUnitStats(unitName);
                }
                else if(type == eUnits)
                {
                    //set enemy stats
                    EnemyUnit eU = unit.GetComponent<EnemyUnit>();
                    eU.stats = GetBasicUnitStats(unitName);
                }
             

                //if we have any pugrades add them now
                //add upgrages to unit stats
            }
        }
    }
}
