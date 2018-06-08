using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    //public float bulletSpeed = 5f;
    //public float bulletLifeTime = 10f;
    // Use this for initialization
    public Material[] _materials;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        //bulletLifeTime -= Time.deltaTime;
        //if (bulletLifeTime <=0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            VibrationScript vibrationScript = other.GetComponent<VibrationScript>();

            if (gameObject.transform.GetComponent<Renderer>().material.name.Contains(_materials[1].name)
                && other.gameObject.GetComponentInParent<MovementScript>().isSecondaryMoveController)
            {
                gameObject.transform.GetComponent<Renderer>().material = _materials[0];
                vibrationScript.StartCoroutine(vibrationScript.Vibrate());
            }
            else if (gameObject.transform.GetComponent<Renderer>().material.name.Contains(_materials[2].name)
                && !other.gameObject.GetComponentInParent<MovementScript>().isSecondaryMoveController)
            {
                gameObject.transform.GetComponent<Renderer>().material = _materials[0];
                vibrationScript.StartCoroutine(vibrationScript.Vibrate());
            }
        }
        //Destroy(gameObject);
    }
}
