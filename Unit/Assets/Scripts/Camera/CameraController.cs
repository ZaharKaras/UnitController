using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float minY;
    public float maxY;
    public Vector2 panLimint;

    public float scrollSpeed;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 scroolPos = pos;


        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("a") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("d") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 100f;

        pos.x = Mathf.Clamp(pos.x, -panLimint.x, panLimint.x);
        scroolPos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.y = Mathf.Clamp(pos.y, 5, 30);
        pos.z = Mathf.Clamp (pos.z, -panLimint.y, panLimint.y);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(pos.y * 3, 0f, 0f);
    }
}
