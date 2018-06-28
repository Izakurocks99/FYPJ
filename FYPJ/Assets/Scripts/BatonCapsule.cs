using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonCapsule : MonoBehaviour {
    [SerializeField]
    private BatonCapsuleFollower _batonCapsuleFollowerPrefab;
    private PlayerStickScript thisStick;

    private void Start()
    {
        thisStick = GetComponentInParent<PlayerStickScript>();
        SpawnBatCapsuleFollower();
    }

    private void SpawnBatCapsuleFollower()
    {
        var follower = Instantiate(_batonCapsuleFollowerPrefab);
        follower.transform.position = transform.position;
        follower.SetFollowTarget(this);
        thisStick.BatonFollowers.Add(follower);
    }
}
