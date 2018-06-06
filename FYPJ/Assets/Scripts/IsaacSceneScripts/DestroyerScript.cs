using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class DestroyerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Destructable")
        {
            Destroy(other.gameObject);
            StartCoroutine(Vibrate());
        }
    }

    IEnumerator Vibrate()
    {

        PS4Input.MoveSetVibration(0, 1, 128);
        PS4Input.MoveSetVibration(0, 0, 128);
        yield return new WaitForSeconds(0.1f);
        PS4Input.MoveSetVibration(0, 1, 0);
        PS4Input.MoveSetVibration(0, 0, 0);
    }
}
