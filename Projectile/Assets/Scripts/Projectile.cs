using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        Stone stone = collision.gameObject.GetComponent<Stone>();

        if( stone != null )
        {
            stone.TakeDamage();
            Destroy(gameObject);
        }
    }
}
