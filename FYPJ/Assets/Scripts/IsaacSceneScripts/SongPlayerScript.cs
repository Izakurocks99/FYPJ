using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayerScript : MonoBehaviour {
    [SerializeField]
    AudioSource audiosource = null;
    [SerializeField]
    List<SongScriptableObject> songs = null;

    Dictionary<string,AudioClip> songlist;
    [SerializeField]
    bool getSavedSong = true;
    [SerializeField]
    AudioClip defaultSong;
	// Use this for initialization
	void Start () {
        songlist = new Dictionary<string, AudioClip>();

        foreach (SongScriptableObject var in songs)
        {
            songlist.Add(var.title, var.audioClip);
        }

        if (audiosource.isPlaying)
            audiosource.Stop();

        if (getSavedSong)
        {
            PlaySong(PlayerPrefs.GetString("test"));
        }
        //else
        //{
        //    audiosource.clip = defaultSong;
        //    audiosource.Play();
        //}
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PlaySong(string songname)
    {
        GetComponent<AudioSource>().clip = songlist[songname];
        GetComponent<AudioSource>().PlayDelayed(3f);
        audiosource.clip = songlist[songname];
        audiosource.PlayDelayed(3+2);
    }
}
