using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float smoothing;
    public Vector2 minPosition, maxPosition;//variables for camera clamping
    PersistentStats stats;
    bool bound = false;
    bool done = false;

    private void Start()
    {
        stats = FindObjectOfType<PersistentStats>();
        bound = false;
    }
    void LateUpdate()
    {
        if(SceneManager.GetActiveScene().name == "MapGeneration" && !bound)
        {
            minPosition = new Vector2(-stats.width/2, -stats.height/2);
            maxPosition = new Vector2(stats.width/2, stats.height/2);
            bound = true;
        }
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
