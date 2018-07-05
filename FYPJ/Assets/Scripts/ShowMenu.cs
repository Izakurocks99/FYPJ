using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour {

    bool showing, showingAudioMenu, showingDifficultyMenu, hiding, hidingAudioMenu, hidingDifficultyMenu, audioPriority, difficultyPriority;
    public float _ScrollLenght, transitionSpeed;
    public Vector3 showingPosition, audioShowingPos,difficultyShowingPos, hidingPosition, audioHidingPos, difficultyHidingPos;
	public GameObject audioMenu, difficultyMenu, exitMenu;


	// Use this for initialization
	void Start () {

        hidingPosition = transform.localPosition;
		audioHidingPos = audioMenu.transform.localPosition;
		difficultyHidingPos = difficultyMenu.transform.localPosition;
        showing = false;
        hiding = true;
		showingAudioMenu = false;
		showingDifficultyMenu = false;
		hidingAudioMenu = true;
		hidingDifficultyMenu = true;
		
	}
	

	public void ShowexitMenu()
	{
		exitMenu.SetActive(true);
	}

	public void ExitNo()
	{
		exitMenu.SetActive(false);
	}

	public void ExitYes()
	{
		Application.Quit();
	}

    public void Show()
    {
        if(hiding)
            StartCoroutine("ShowOptionMenu");

        if (showing)
            StartCoroutine("HideOptionMenu");
    }

	public void ShowAudio()
	{
		if (hidingAudioMenu)
			StartCoroutine("ShowAudioMenu");
		if (showingAudioMenu)
			StartCoroutine("HideAudioMenu");
	}

	public void HideAudio()
	{
		StartCoroutine("HideAudioMenu");
	}


	public void ShowDifficulty()
	{
		if(hidingDifficultyMenu)
			StartCoroutine("ShowDifficultyMenu");
		if (showingDifficultyMenu)
			StartCoroutine("HideDifficultyMenu");
	}

	public void HideDifficulty()
	{
		StartCoroutine("HideDifficultyMenu");
	}



    IEnumerator ShowOptionMenu()
    {
        showing = false;

        while(!showing)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, showingPosition, Time.deltaTime * transitionSpeed);

            if (Vector3.Distance(transform.localPosition, showingPosition) < 1)
                showing = true;

            yield return 0;


        }


        yield break;
    }

    IEnumerator HideOptionMenu()
    {
        hiding = false;

		while (!hiding)
		{

			transform.localPosition = Vector3.Lerp(transform.localPosition, hidingPosition, Time.deltaTime * transitionSpeed);

			if (Vector3.Distance(transform.localPosition, hidingPosition) < 1)
				hiding = true;

			yield return 0;


		}


        yield break;
    }


	IEnumerator ShowAudioMenu()
	{
		showingAudioMenu = false;
		

		while (!showingAudioMenu)
		{
			audioMenu.transform.localPosition = Vector3.Lerp(audioMenu.transform.localPosition, audioShowingPos, Time.deltaTime * transitionSpeed);

			if (Vector3.Distance(audioMenu.transform.localPosition, audioShowingPos) < 1 || !hidingAudioMenu)
				showingAudioMenu = true;

			yield return 0;


		}


		yield break;
	}



	IEnumerator HideAudioMenu()
	{
		hidingAudioMenu = false;
		

		while (!hidingAudioMenu)
		{
			audioMenu.transform.localPosition = Vector3.Lerp(audioMenu.transform.localPosition, audioHidingPos, Time.deltaTime * transitionSpeed);

			if (Vector3.Distance(audioMenu.transform.localPosition, audioHidingPos) < 1)
				hidingAudioMenu = true;

			yield return 0;


		}


		yield break;
	}



	IEnumerator ShowDifficultyMenu()
	{
		showingDifficultyMenu = false;
		

		while (!showingDifficultyMenu)
		{
			difficultyMenu.transform.localPosition = Vector3.Lerp(difficultyMenu.transform.localPosition, difficultyShowingPos, Time.deltaTime * transitionSpeed);

			if (Vector3.Distance(difficultyMenu.transform.localPosition, difficultyShowingPos) < 1 || !hidingDifficultyMenu)
				showingDifficultyMenu = true;

			yield return 0;


		}


		yield break;
	}



	IEnumerator HideDifficultyMenu()
	{
		hidingDifficultyMenu = false;
		

		while (!hidingDifficultyMenu)
		{
			difficultyMenu.transform.localPosition = Vector3.Lerp(difficultyMenu.transform.localPosition, difficultyHidingPos, Time.deltaTime * transitionSpeed);

			if (Vector3.Distance(difficultyMenu.transform.localPosition, difficultyHidingPos) < 1)
				hidingDifficultyMenu = true;

			yield return 0;


		}


		yield break;
	}
}
