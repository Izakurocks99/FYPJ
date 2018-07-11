using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchScene : MonoBehaviour {

	public GameObject loadingScreen;

	// Use this for initialization
	void Start ()
	{
		loadingScreen.GetComponent<SceneSwitch>().Fade();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
