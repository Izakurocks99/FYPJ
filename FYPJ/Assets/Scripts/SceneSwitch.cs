﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour {

	public GameObject loadingScreen;
	public GameObject loadingImage;
	public string sceneName;
	private float count = 0;
	private Color color;
    bool switching = false;

	void Start()
	{
        switching = false;
        //loadingScreen = GameObject.Find("LoadingScreen");
        //loadingImage = GameObject.Find("LoadingImage");

        StartCoroutine(FadeOut());
    }

    private void Update()
	{
		//if (Input.anyKeyDown && SceneManager.GetActiveScene().buildIndex == 0)
		//{
		//	LoadScene();
		//}
	}

    public void LoadScene(string scene)
    {
        if (!switching)
        {

            color = new Color(1, 1, 1, 0);
            loadingScreen.SetActive(true);
            StartCoroutine(FadeIn(scene));
            switching = true;
        }
    }

    IEnumerator FadeIn(string scene)
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
        SceneManager.LoadSceneAsync(scene);
        yield break;
    }

    public void LoadScene()
	{
        if(!switching)
        {

		color = new Color(1,1,1,0);
		loadingScreen.SetActive(true);
		StartCoroutine(FadeIn());
        switching = true;
        }
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
