using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    ProceduralGeneration PCG;

    public List<Transform> targets;
    public Vector3 offset;

    void Start()
    {
        PCG = FindObjectOfType<ProceduralGeneration>();

    }
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targets = GetTargets();
            if (targets.Count == 0)
                return;
            Vector3 centerPoint = GetCenterPoint();
            Vector3 newPosition = centerPoint + offset;
            transform.position = newPosition;
        }
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i=0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    List<Transform> GetTargets()
    {
        List<Transform> targetsToEncapsulate = new List<Transform>();
        foreach (Transform tile in PCG.transform)
        {
            targetsToEncapsulate.Add(tile);
        }

        return targetsToEncapsulate;
    }
}


