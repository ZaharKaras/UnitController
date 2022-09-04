using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int healPoint = 1000;

    // Update is called once per frame
    void Update()
    {
        if(healPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        healPoint -= 10;
    }
}
