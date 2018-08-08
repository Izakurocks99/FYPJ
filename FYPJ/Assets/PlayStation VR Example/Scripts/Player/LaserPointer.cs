using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
    public LayerMask layerMask;
    public int max_distance = 100;
    private LineRenderer line;
    private RaycastHit hit;
    SelectionVisualizer stickSelection;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Keep the pointer aimed at whatever it's hitting, or else just keep going for 100 units
    void Update()
    {
        //Physics.Raycast(transform.position, transform.forward, out hit, max_distance, uiLayerMask);
        if (Physics.Raycast(transform.position, transform.forward, out hit, max_distance, layerMask))
        {
            line.SetPosition(1, Vector3.forward * hit.distance);
            line.startColor = Color.green;
            line.endColor = Color.green;

            if (hit.collider.gameObject.GetComponentInParent<SelectionVisualizer>())
            {
                stickSelection = hit.collider.gameObject.GetComponentInParent<SelectionVisualizer>();
                stickSelection.selected = true;
            }
            else
                stickSelection.selected = false;
        }
        else
        {
            line.SetPosition(1, Vector3.forward * 100);
            line.startColor = Color.red;
            line.endColor = Color.red;
        }
    }

    public Collider LineRaycast()
    {
        //Physics.Raycast(transform.position, transform.forward, out hit, max_distance, uiLayerMask);
        if (Physics.Raycast(transform.position, transform.forward, out hit, max_distance, layerMask))
            return hit.collider;
        else
            return null;
    }
}
