using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit")]
public class Unit : ScriptableObject
{
    public enum unitType
    {
        Worker,
        Warrior,
        Healer
    }

    [Space(15)]
    [Header("Unit settings")]
    public unitType type;

    public new string name;
    
    public GameObject playerPrefabs;
    public GameObject enemyPrefabs;

    [Space(15)]
    [Header("Unit stats")]
    [Space(40)]

    public UnitStatType.Base baseStats;

}
