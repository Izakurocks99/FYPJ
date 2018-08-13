using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandTargetMenuScript : MonoBehaviour {

    [SerializeField]
    Button fourSpawners = null;
    [SerializeField]
    Button eightSpawners = null;

    // Use this for initialization
    void Start ()
    {
        if (PlayerPrefs.GetInt("eightspawners") == 0)
            fourSpawners.gameObject.SetActive(false);
        else
            eightSpawners.gameObject.SetActive(false);

        Debug.Assert(fourSpawners);
        Debug.Assert(eightSpawners);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
