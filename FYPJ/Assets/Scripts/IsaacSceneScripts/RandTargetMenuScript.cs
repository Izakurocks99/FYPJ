using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandTargetMenuScript : MonoBehaviour {

    [SerializeField]
    Button straightButton = null;
    [SerializeField]
    Button RandomButton = null;

    // Use this for initialization
    void Start ()
    {
        if (PlayerPrefs.GetInt("randomarea") == 1)
            straightButton.gameObject.SetActive(false);
        else
            RandomButton.gameObject.SetActive(false);

        Debug.Assert(straightButton);
        Debug.Assert(RandomButton);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
