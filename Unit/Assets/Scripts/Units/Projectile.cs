using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if(transform.position == endPosition)
        {
            transform.position = startPosition;
        }

    }

    public void Fire(Vector3 destination)
    {
        endPosition = destination;
    }
}
