using UnityEngine;
using System.Collections;

public class SpawnParticles : MonoBehaviour {

    public GameObject _leftWing, _rightWing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateParticles()
    {
        _leftWing.GetComponent<ParticleSystem>().Play();
        _rightWing.GetComponent<ParticleSystem>().Play();
    }
}
