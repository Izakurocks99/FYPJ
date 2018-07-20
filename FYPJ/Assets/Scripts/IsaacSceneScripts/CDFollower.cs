using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDFollower : MonoBehaviour {
    public Transform lookAt;


    //private void Awake()
    //{
    //    _rigidbody = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    Vector3 destination = _batonFollower.transform.position;
    //    transform.rotation = _batonFollower.transform.rotation;
    //    _rigidbody.transform.rotation = transform.rotation;

    //    _rigidbody.velocity = (destination - _rigidbody.transform.position) * _sensitivity;
    //}

    private void Update()
    {
        Vector3 tempForward = lookAt.position - this.transform.position;
        this.transform.forward = new Vector3(tempForward.x,0,tempForward.z).normalized;
    }

    public void SetFollowTarget(Transform go)
    {
        lookAt = go;
    }
}
