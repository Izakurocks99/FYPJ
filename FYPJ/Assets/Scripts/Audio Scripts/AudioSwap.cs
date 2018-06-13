using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwap : MonoBehaviour {

    public GameObject[] _goContainers;

    void Start() {
    }
    
	void Update () {
	}

    public void ChangeMusic(int _intTrack) {
        this.transform.GetComponent<AudioSource>().Stop();

        List<GameObject> listDelete = new List<GameObject>();

        for (int j = 0; j < _goContainers.Length; j++)
        {
            GameObject go = _goContainers[j];
            foreach (Transform child in go.transform)
                listDelete.Add(child.gameObject);
        }

        for (int i = 0; i < listDelete.Count; i++)
            DestroyImmediate(listDelete[i]);

        this.transform.GetComponent<AudioSource>().clip = this.transform.GetComponent<AudioSampler>()._audioclips[_intTrack];
        this.transform.GetComponent<AudioSource>().Play();

    }
}
