using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float bulletSpeed = 5f;
    public float bulletLifeTime = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <=0)
        {
            Destroy(this);
        }
	}
}
