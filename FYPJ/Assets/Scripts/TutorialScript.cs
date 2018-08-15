using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    [SerializeField]
    List<Sprite> tutorialImages;

    Image targetImage;

    [SerializeField]
    int currImage;
    // Use this for initialization
    void Start () {
        //if (PlayerPrefs.GetInt("showtutorial") == 0)
        //    gameObject.SetActive(false);

        targetImage = gameObject.GetComponent<Image>();
        targetImage.sprite = tutorialImages[currImage];
	}
	
	// Update is called once per frame
	void Update () {

        if (currImage < tutorialImages.Count)
            targetImage.sprite = tutorialImages[currImage];
        else
        {
            targetImage.gameObject.SetActive(false);
        }
    }

    public bool NextImage(int nextImage)
    {
        if (currImage == nextImage - 1)
        {
            currImage = nextImage;
        }
    }
}
