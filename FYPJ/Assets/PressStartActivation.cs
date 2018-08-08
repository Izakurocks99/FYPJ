using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressStartActivation : MonoBehaviour {

	public GameObject pressStartGO;
	Animator anim;

	void Start ()
	{
		anim = pressStartGO.GetComponent<Animator>();

		StartCoroutine("Init");
	}

	IEnumerator Init()
	{
		yield return new WaitForSeconds(4.25f);
		pressStartGO.SetActive(true);

		yield break;
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			anim.Play("PressStartSelected");		
		}
	}

	IEnumerator SceneSwitch()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(1);
	}
}
