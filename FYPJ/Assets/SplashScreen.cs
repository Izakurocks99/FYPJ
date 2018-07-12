using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	Color color;
	public GameObject startScreen;

	// Use this for initialization
	void Start ()
	{
		color.r = 1;
		color.g = 1;
		color.b = 1;
		color.a = 0;
		StartCoroutine("Disapear");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Disapear()
	{
		float _count = 0;

		while (gameObject.GetComponent<SpriteRenderer>().color.a != 1)
		{
			color.a = Mathf.Lerp(0, 1, _count);
			_count += Time.deltaTime;
			Debug.Log(color.a);
			gameObject.GetComponent<SpriteRenderer>().color = color;
			yield return null;
		}

		_count = 0;

		while (gameObject.GetComponent<SpriteRenderer>().color.a != 0)
		{		
			color.a = Mathf.Lerp(1, 0, _count);
			_count += Time.deltaTime;
			Debug.Log(color.a);
			gameObject.GetComponent<SpriteRenderer>().color = color;
			yield return null;
		}

		yield return new WaitForSeconds(1);
		startScreen.SetActive(true);

		yield break;
	}
}
