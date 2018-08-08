using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {

    Image image;
    [SerializeField]
    GameObject loading;
    [SerializeField]
    List<Sprite> creditsImages = null;

    Image targetImage;

    [SerializeField]
    int currImage;

    [SerializeField]
    float switchTime = 0;
    float countdown;

    [SerializeField]
    string lobbySceneName = null;

    [SerializeField]
    SceneSwitch sceneSwitch = null;

    bool start = false;
    // Use this for initialization
    void Start () {

        targetImage = gameObject.GetComponent<Image>();
        targetImage.sprite = creditsImages[currImage];

        countdown = switchTime;
    }
	
	// Update is called once per frame
	void Update () {

        if(!start && !loading.activeInHierarchy)
        {
            start = true;
        }

        if (start)
            countdown -= Time.deltaTime;
		if(countdown <= 0)
        {
            countdown = switchTime;
            currImage++;
            if (currImage >= creditsImages.Count)
                sceneSwitch.LoadScene(lobbySceneName);
            else
            targetImage.sprite = creditsImages[currImage];
        }
	}
}
