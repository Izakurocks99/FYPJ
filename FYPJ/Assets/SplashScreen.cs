using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SplashScreen : MonoBehaviour {

	Color color;

	private void Start()
	{
		StartCoroutine("Play");
		this.GetComponent<Image>().color 
	}

	IEnumerator Play()
	{
		float _count = 0;
		while (_count < 1)
		{
			this.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Lerp(0, 1, _count));
			_count += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		yield break;
	}
}
