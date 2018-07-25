using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonCapsule : MonoBehaviour {
    [SerializeField]
    private BatonCapsuleFollower _batonCapsuleFollowerPrefab = null;
    public PlayerStickScript thisStick;

    private void Start()
    {
        thisStick = GetComponentInParent<PlayerStickScript>();
        SpawnBatCapsuleFollower();
    }

    private void SpawnBatCapsuleFollower()
    {
        var follower = Instantiate(_batonCapsuleFollowerPrefab);
        follower.transform.position = transform.position;
        follower.transform.localScale = transform.lossyScale;
        follower.SetFollowTarget(this);
        thisStick.BatonFollowers.Add(follower);
    }
}
