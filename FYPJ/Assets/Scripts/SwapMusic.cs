using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMusic : MonoBehaviour {

    public GameObject _goContainer;
    
	void Update () {
		
	}

    public void ChangeMusic(int _intTrack)
    {
        this.transform.GetComponent<AudioSource>().Stop();

        List<GameObject> listDelete = new List<GameObject>();
        foreach (Transform child in _goContainer.transform)
            listDelete.Add(child.gameObject);

        for (int i = 0; i < listDelete.Count; i++)
            DestroyImmediate(listDelete[i]);

        this.transform.GetComponent<AudioSource>().clip = this.transform.GetComponent<AudioPeer>()._audioclips[_intTrack];
        this.transform.GetComponent<AudioSource>().Play();
    }
}
