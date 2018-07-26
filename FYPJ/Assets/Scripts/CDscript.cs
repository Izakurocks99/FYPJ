using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDscript : MonoBehaviour
{

    public SongScriptableObject song;
    public Text Title;
    public Text Description;
    //public GameObject platform, player;
    public GameObject audioSource;
    //, loadingScreen;
    public Vector3 originalScale;

    public SelectSongsComponent parent;
    public float maxDist;
    public float minDist;
    public float selectDist;


    // Use this for initialization
    void Start()
    {
        //Title = GameObject.Find("SongsSelection/Screen/Canvas/Title").GetComponent<Text>();
        //Description = GameObject.Find("SongsSelection/Screen/Canvas/Description").GetComponent<Text>();
        //platform = GameObject.Find("SongsSelection/platform");
        //player = GameObject.Find("Player");
        //audioSource = GameObject.Find("SongsSelection/Screen/Audio Source");
        //loadingScreen = GameObject.Find("Player/MainCamera");
    }

    // Update is called once per frame
    void Update()
    {

        //transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, (((-transform.position.z + 3) / 6))/.7f);
        float a = (gameObject.transform.position - parent.frontPoint).magnitude;
        transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, a / (maxDist - minDist));
        transform.localRotation = Quaternion.identity;

        if (a < selectDist)
        {
            parent.currCD = this;
            Title.text = song.title;
            Description.text = song.description;
            if (song.audioClip != null)
                audioSource.GetComponent<AudioSource>().clip = song.audioClip;
            if (audioSource.GetComponent<AudioSource>().isPlaying == false)
                audioSource.GetComponent<AudioSource>().Play();

        }
    }

    public void Hold(Transform lookat)
    {
        parent.Select();
        parent.follower.lookAt = lookat;
    }

    public void Release()
    {
        parent.Release();
    }

    public IEnumerator LaunchSong()
    {

        bool _waiting = true;
        float _timer = 0;
        while (_waiting)
        {
            parent.platform.transform.position += Vector3.up * Time.deltaTime * 3;
            parent.player.transform.position += Vector3.up * Time.deltaTime * 3;

            GetComponent<Renderer>().material.SetFloat("Vector1_798353CA", Mathf.Lerp(_timer - 1, 1, _timer / 5));

            if (_timer > 2)
            {
                _waiting = false;
                parent.loadingScreen.GetComponent<SceneSwitch>().LoadScene();
            }

            _timer += Time.deltaTime;

            yield return 0;

        }




        //	yield break;
        //}
    }
}
