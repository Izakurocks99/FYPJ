using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Destroy(this);
    //    if (other.gameObject.tag == "Player")
    //    {
    //        StartCoroutine(Vibrate());
    //    }
    //}

    //IEnumerator Vibrate()
    //{

    //    PS4Input.MoveSetVibration(0, 1, 128);
    //    PS4Input.MoveSetVibration(0, 0, 128);
    //    yield return new WaitForSeconds(0.1f);
    //    PS4Input.MoveSetVibration(0, 1, 0);
    //    PS4Input.MoveSetVibration(0, 0, 0);
    //}
}
