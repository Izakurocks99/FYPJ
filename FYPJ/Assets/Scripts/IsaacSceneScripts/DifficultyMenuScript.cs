using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyMenuScript : MonoBehaviour {

    [SerializeField]
    SelectSongsComponent selectSongsComponent = null;

    DifficultyButtonsScript[] difficultyButtons;

	// Use this for initialization
	void Start () {
        Debug.Assert(selectSongsComponent);
        difficultyButtons = GetComponentsInChildren<DifficultyButtonsScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySong()
    {
        StartCoroutine(selectSongsComponent.currCD.LaunchSong());
    }

    public void SelectDifficulty(DifficultyButtonsScript buttonsScript)
    {
        foreach (DifficultyButtonsScript button in difficultyButtons)
        {
            if (button != buttonsScript)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                PlaySong();

                FindObjectOfType<TutorialScript>().NextImage(5);
            }
        }
    }
}
