using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour {

    public float lifeTime = 10f;
    public float bulletSpeed = 10f;

    public float minMass = 1f;
    public float maxMass = 50f;
    public float scaleMultiplier = 0.05f;

    Rigidbody rb;
    public PlayerScript player;

    bool isHit = false;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();

        //rb.mass = Random.Range(minMass, maxMass);
        //float scale = rb.mass * scaleMultiplier;
        //gameObject.transform.localScale = new Vector3(scale, scale, scale);
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
            if (!isHit)
                player.ModifyCombo(false);
        }

        if (!isHit)
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
	}

    private void OnCollisionEnter(Collision collision)
    {
        isHit = true;
        rb.useGravity = true;
        gameObject.GetComponent<Collider>().enabled = false;
        player.ModifyCombo(true);
    }
}
