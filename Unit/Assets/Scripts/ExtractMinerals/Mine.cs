using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mine : MonoBehaviour
{
    public int minerals = 100;

    private void Update()
    { 
        if(minerals <= 0)
        {
            Destroy(gameObject);
        }
    }
    public int GetMinerals()
    {
        minerals -= 10;

        return 10; 
    }

}
