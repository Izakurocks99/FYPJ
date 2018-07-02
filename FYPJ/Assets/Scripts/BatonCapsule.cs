using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonCapsule : MonoBehaviour {
    [SerializeField]
    private BatonCapsuleFollower _batonCapsuleFollowerPrefab;
    public PlayerStickScript thisStick;

    private void Start()
    {
        thisStick = GetComponentInParent<PlayerStickScript>();
        SpawnBatCapsuleFollower();
    }

    private void SpawnBatCapsuleFollower()
    {
        var follower = Instantiate(_batonCapsuleFollowerPrefab,gameObject.transform);
        follower.transform.position = transform.position;
        follower.SetFollowTarget(this);
        thisStick.BatonFollowers.Add(follower);
    }
}
