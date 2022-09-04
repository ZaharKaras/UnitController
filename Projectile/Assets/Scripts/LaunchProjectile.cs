using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 1000f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Transform ballPosition = transform;

            GameObject ball = Instantiate(projectile, transform.position,
                                                      transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                                 (0, launchVelocity, 0));
        }
    }
}
