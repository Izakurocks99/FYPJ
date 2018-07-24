using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDFollower : MonoBehaviour {
    public Transform lookAt;

    private void Update()
    {
        Vector3 tempForward = transform.forward;
        if (lookAt)
            tempForward = lookAt.position - this.transform.position;
        this.transform.forward = new Vector3(tempForward.x,0,tempForward.z).normalized;
    }

    public void SetFollowTarget(Transform go)
    {
        lookAt = go;
    }
}
