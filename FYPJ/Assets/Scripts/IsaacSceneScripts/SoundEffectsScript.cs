using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffectVar
{
    public string clipName;
    public AudioClip audioClip;
}

public class SoundEffectsScript : MonoBehaviour {
    [SerializeField]
    List<SoundEffectVar> soundEffectsList;
    Dictionary<string, AudioClip> audioDict;
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioDict = new Dictionary<string, AudioClip>();
        foreach(SoundEffectVar var in soundEffectsList)
        {
            audioDict.Add(var.clipName, var.audioClip);
        }

        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string soundName)
    {
        audioSource.clip = audioDict[soundName];
        audioSource.Play();
    }
}
