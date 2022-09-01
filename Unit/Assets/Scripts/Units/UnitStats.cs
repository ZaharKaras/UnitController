using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitStats", menuName = "New UnitStats")]
public class UnitStats : ScriptableObject
{
    public enum unitType
    {
        Worker,
        Warrior,
        Archer
    }

    [Space(15)]
    [Header("UnitStats settings")]
    public unitType type;
    
    public GameObject playerPrefabs;
    public GameObject enemyPrefabs;

    [Space(15)]
    [Header("UnitStats stats")]
    [Space(40)]

    public UnitStatType.Base baseStats;

}
