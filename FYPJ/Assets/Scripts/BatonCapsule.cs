using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonCapsule : MonoBehaviour {
    [SerializeField]
    private BatonCapsuleFollower _batonCapsuleFollowerPrefab = null;
    public PlayerStickScript thisStick;
    public bool activeFollower;

    private void Start()
    {
        thisStick = GetComponentInParent<PlayerStickScript>();
        SpawnBatCapsuleFollower();
    }

    public void SpawnBatCapsuleFollower()
    {
        var follower = Instantiate(_batonCapsuleFollowerPrefab);
        follower.transform.position = transform.position;
        follower.transform.localScale = transform.lossyScale;
        follower.SetFollowTarget(this);
        if (!activeFollower)
            follower.gameObject.SetActive(false);

        thisStick.BatonFollowers.Add(follower);
    }
}
