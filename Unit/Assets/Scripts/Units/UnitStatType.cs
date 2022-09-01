using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatType : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public string name;
        public float cost, aggroRange, attackSpeed,attackRange, damage, hp, armor, speed;
    }
}
