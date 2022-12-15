using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitions : MonoBehaviour
{
    private CameraController cam;
    public Vector2 newCamMinPos, newCamMaxPos;//where the camera will be clamped on
    public Vector3 movePlayer;//small teleporting of player
    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.minPosition = newCamMinPos;
            cam.maxPosition = newCamMaxPos;
            other.transform.position += movePlayer;
        }
    }
}
