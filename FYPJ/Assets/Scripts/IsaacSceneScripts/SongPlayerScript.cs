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

    public string songtitle;
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
            songtitle = PlayerPrefs.GetString("test");
            PlaySong(songtitle);
        }
        //else
        //{
        //    audiosource.clip = defaultSong;
        //    audiosource.Play();
        //}
    }

    //bool savesong = true;
    // Update is called once per frame
    void Update () {
        //if(!audiosource.isPlaying && savesong)
        //{
        //    savesong = false;
        //    int score = FindObjectOfType<PlayerStats>()._intPlayerScoring;
        //    //if have key
        //    if (PlayerPrefs.HasKey(songtitle + "highscore"))
        //    {
        //        int highscore = PlayerPrefs.GetInt(songtitle + "highscore");
        //        //compare if curr score is higher
        //        if (score > highscore)
        //        {
        //            //set curr score as highscore
        //            PlayerPrefs.SetInt(songtitle + "highscore", score);
        //        }

        //    }
        //    else
        //    {

        //    //if have no key
        //    PlayerPrefs.SetInt(songtitle + "highscore", score);
        //    }
        //}

    }

    public void PlaySong(string songname)
    {
        GetComponent<AudioSource>().clip = songlist[songname];
        GetComponent<AudioSource>().PlayDelayed(3f);
        audiosource.clip = songlist[songname];
        audiosource.PlayDelayed(3+2);
    }
}
