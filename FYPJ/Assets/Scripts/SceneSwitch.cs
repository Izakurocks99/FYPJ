using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour {

	public GameObject loadingScreen;
	public GameObject loadingImage;
	public string sceneName = "Lobby";
	private float count = 0;
	private Color color;

	void Start()
	{
		//loadingScreen = GameObject.Find("LoadingScreen");
		//loadingImage = GameObject.Find("LoadingImage");
		
		Fade();
	}

	private void Update()
	{
		//if (Input.anyKeyDown && SceneManager.GetActiveScene().buildIndex == 0)
		//{
		//	LoadScene();
		//}
	}

	public void LoadScene()
	{
		color = new Color(1,1,1,0);
		loadingScreen.SetActive(true);
		StartCoroutine("FadeIn");
		
	}

	public void Fade()
	{
		StartCoroutine("FadeOut");
	}

	IEnumerator FadeIn()
	{
		color = new Color(1, 1, 1, 0);
		count = 0;
		while (color.a != 1)
		{
			color.a = Mathf.Lerp(0, 1, count);
			loadingImage.GetComponent<Image>().color = color;
			
			count += Time.deltaTime;

			yield return null;
		}
		SceneManager.LoadSceneAsync(sceneName);
		yield break;
	}

	IEnumerator FadeOut()
	{
		color = new Color(1,1,1,1);
		count = 0;
		while (color.a != 0)
		{
			color.a = Mathf.Lerp(1, 0, count);
			loadingImage.GetComponent<Image>().color = color;
			count += Time.deltaTime;
            if (color.a <= 0)
            {
                loadingScreen.SetActive(false);
            }

            yield return null;
		}
		yield break;
	}
}
