using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			LoadScene();
		}
	}

	void LoadScene()
	{
		SceneManager.LoadScene("Lobby");
	}
}
