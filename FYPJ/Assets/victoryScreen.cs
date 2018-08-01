using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class victoryScreen : MonoBehaviour {

	Text vicTxt;

	private void Start()
	{
		vicTxt = this.transform.GetComponentInChildren<Text>();
		vicTxt.color = new Color(1, 1, 1, 0);
	}

	void Activate()
	{
		StartCoroutine("FadeIn");
	}

	IEnumerator FadeIn()
	{
		ParticleSystem[] _particleList = GetComponentsInChildren<ParticleSystem>();

		for (int i = 0; i < _particleList.Length; i++)
			_particleList[i].Play();

		float _count = 0;
		while(_count < 1)
		{
			vicTxt.color = new Color( 1, 1, 1, Mathf.Lerp(0,1,_count)) ;
			_count += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(3);
		//SceneManager.LoadScene(0);

		yield break;
	}
}
