using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerUnits;
    public Transform enemyUnits;

    private void Awake()
    {
        instance = this;
        UnitHandler.instance.SetBasicUnitStats(playerUnits);
        UnitHandler.instance.SetBasicUnitStats(enemyUnits);
    }
    void Start()
    {
        
    }

    void Update()
    {
        InputHandler.instance.HandleUnitMovement();
    }
}
