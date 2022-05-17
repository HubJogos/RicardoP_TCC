using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float smoothing;
    public Vector2 minPosition, maxPosition;//variables for camera clamping
    bool done = false;


    void LateUpdate()
    {
        if (!done && FindObjectOfType<PlayerScript>() != null)
        {
            target = FindObjectOfType<PlayerScript>().gameObject;
            done = true;
        }

        if (done)
        {
            if (transform.position != target.transform.position)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
            }
        }
        
    }
}
