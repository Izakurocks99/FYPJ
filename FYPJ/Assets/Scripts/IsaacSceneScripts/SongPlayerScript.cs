using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayerScript : MonoBehaviour {
    [SerializeField]
    AudioSource audiosource = null;
    [SerializeField]
    List<SongScriptableObject> songs = null;

    Dictionary<string,AudioClip> songlist;

	// Use this for initialization
	void Start () {
        songlist = new Dictionary<string, AudioClip>();

        foreach (SongScriptableObject var in songs)
        {
            songlist.Add(var.title, var.audioClip);
        }

        PlaySong(PlayerPrefs.GetString("test"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySong(string songname)
    {
        audiosource.clip = songlist[songname];
        audiosource.Play();
    }
}
