using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
