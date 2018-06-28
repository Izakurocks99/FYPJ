using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerScript : MonoBehaviour {

    public SimpleBullet BulletPrefab;
    public GameObject PlayerObj;
    public float spawnDelay;
    PlayerScript player;

    // Use this for initializatio
    void Start () {
        player = FindObjectOfType<PlayerScript>();
        StartCoroutine(SpawnBullet());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnBullet()
    {
        for (; ; )
        {
            Vector3 randomNoise = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f), Random.Range(-0.5f, 0.5f));
            SimpleBullet SpawnedBullet = Instantiate(BulletPrefab);
            SpawnedBullet.transform.position = transform.position;
            SpawnedBullet.transform.forward = ((PlayerObj.transform.position + randomNoise) - transform.position).normalized;
            SpawnedBullet.player = player;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
