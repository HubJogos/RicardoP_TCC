using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;
    bool done = false;

    private void LateUpdate()
    {
        if (!done && FindObjectOfType<PlayerScript>()!=null)
        {
            player = FindObjectOfType<PlayerScript>().gameObject.transform;
            done = true;
        }
        else if (done)
        {
            Vector3 newPos = player.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        

    }
}
