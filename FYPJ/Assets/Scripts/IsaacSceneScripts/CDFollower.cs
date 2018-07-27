using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDFollower : MonoBehaviour {
    public Transform lookAt;

    private void Update()
    {
        if (lookAt)
        {
            Vector3 tempForward;
            tempForward = lookAt.position - this.transform.position;
            //this.transform.forward = new Vector3(tempForward.x, 0, tempForward.z).normalized;
            tempForward = new Vector3(tempForward.x, 0, tempForward.z).normalized;
            if (tempForward.sqrMagnitude != 0)
            {
                transform.forward = tempForward;
            }
        }
    }

    public void SetFollowTarget(Transform go)
    {
        lookAt = go;
    }
}
