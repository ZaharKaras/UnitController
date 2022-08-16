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

    public bool isPlayerUnit;

    public unitType type;

    public new string name;
    
    public GameObject playerPrefabs;
    public GameObject enemyPrefabs;

    public int cost;
    public int damage;
    public int hp;
    public int armor;
}
