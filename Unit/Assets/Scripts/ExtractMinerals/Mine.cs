using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mine : MonoBehaviour
{
    public int minerals = 100;
    public int GetMinerals()
    {
        minerals -= 10;

        return 10; 
    }
}
