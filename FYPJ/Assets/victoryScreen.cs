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
		//Activate();
	}

	public void Activate()
	{
		StartCoroutine("FadeIn");
	}

	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(2);
		ParticleSystem[] _particleList = GetComponentsInChildren<ParticleSystem>();

		float _count = 0;
		while(_count < 1)
		{
			vicTxt.color = new Color( 1, 1, 1, Mathf.Lerp(0,1,_count)) ;
			_count += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(.4f);

		for (int i = 0; i < _particleList.Length; i++)
			_particleList[i].Play();


		yield return new WaitForSeconds(3);
        FindObjectOfType<SceneSwitch>().LoadScene();
		//SceneManager.LoadScene(0);

		yield break;
	}
}
