using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonCapsuleFollower : MonoBehaviour {
    private BatonCapsule _batonFollower;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    [SerializeField]
    private float _sensitivity = 100f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 destination = _batonFollower.transform.position;
        transform.rotation = _batonFollower.transform.rotation;
        _rigidbody.transform.rotation = transform.rotation;

        _rigidbody.velocity = (destination - _rigidbody.transform.position) * _sensitivity;
    }

    public void SetFollowTarget(BatonCapsule batFollower)
    {
        _batonFollower = batFollower;
    }
}
