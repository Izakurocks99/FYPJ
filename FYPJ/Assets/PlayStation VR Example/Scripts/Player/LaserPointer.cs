﻿using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
    public LayerMask uiLayerMask;
    public int max_distance = 100;
    private LineRenderer line;
    private RaycastHit hit;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Keep the pointer aimed at whatever it's hitting, or else just keep going for 100 units
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, max_distance, uiLayerMask))
        {
            line.SetPosition(1, Vector3.forward * hit.distance);
            line.startColor = Color.green;
            line.endColor = Color.green;
        }
        else
        {
            line.SetPosition(1, Vector3.forward * 100);
            line.startColor = Color.red;
            line.endColor = Color.red;
        }
    }

    public RaycastHit LineRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, max_distance, uiLayerMask);
        return hit;
    }
}
