using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour {

    bool showing, showingAudioMenu, showingDifficultyMenu;
    bool audioPriority, difficultyPriority;
    public float _ScrollLenght, transitionSpeed;
    public Vector3 showingPosition, audioShowingPos,difficultyShowingPos, hidingPosition, audioHidingPos, difficultyHidingPos;
	public GameObject menu, audioMenu, difficultyMenu, exitMenu;


	// Use this for initialization
	void Start () {
        showing = false;
		showingAudioMenu = false;
		showingDifficultyMenu = false;

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
        if (!showing)
        {
            showing = true;
            StartCoroutine(ShowTheMenu(menu, showingPosition));
        }

        else if (showing)
        {
            showing = false;
            StartCoroutine(ShowTheMenu(menu, hidingPosition));

            showingAudioMenu = false;
            StartCoroutine(ShowTheMenu(audioMenu, audioHidingPos));

            showingDifficultyMenu = false;
            StartCoroutine(ShowTheMenu(difficultyMenu, difficultyHidingPos));
        }
    }

	public void ShowAudio()
	{
        if (!showingAudioMenu)
        {
            showingAudioMenu = true;
            StartCoroutine(ShowTheMenu(audioMenu, audioShowingPos));
        }
        else if (showingAudioMenu)
        {
            showingAudioMenu = false;
            StartCoroutine(ShowTheMenu(audioMenu, audioHidingPos));
        }
    }

	public void HideAudio()
    {
        showingAudioMenu = false;
        StartCoroutine(ShowTheMenu(audioMenu, audioHidingPos));
    }


	public void ShowDifficulty()
	{
        if (!showingDifficultyMenu)
        {
            showingDifficultyMenu = true;
            StartCoroutine(ShowTheMenu(difficultyMenu, difficultyShowingPos));
        }
        else if (showingDifficultyMenu)
        {
            showingDifficultyMenu = false;
            StartCoroutine(ShowTheMenu(difficultyMenu, difficultyHidingPos));
        }
    }

	public void HideDifficulty()
    {
        showingDifficultyMenu = false;
        StartCoroutine(ShowTheMenu(difficultyMenu, difficultyHidingPos));
    }


    IEnumerator ShowTheMenu(GameObject themenu, Vector3 topos)
    {
        while (true)
        {
            themenu.transform.localPosition = Vector3.Lerp(themenu.transform.localPosition, topos, Time.deltaTime * transitionSpeed);

            if (Vector3.SqrMagnitude(themenu.transform.localPosition - topos) < 1)
            {
                break;
            }
            else
                yield return null;
        }
    }
}
