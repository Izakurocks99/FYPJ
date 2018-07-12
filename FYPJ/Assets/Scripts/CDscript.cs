using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDscript : MonoBehaviour {

	public SongScriptableObject song;
	public Text Title;
	public Text Description;
	public GameObject platform, player;
	public GameObject audioSource, loadingScreen;
	Vector3 originalScale;


	// Use this for initialization
	void Start ()
	{
		Title = GameObject.Find("Decor/Screen/Canvas/Title").GetComponent<Text>();
		Description = GameObject.Find("Decor/Screen/Canvas/Description").GetComponent<Text>();
		platform = GameObject.Find("Decor/platform");
		player = GameObject.Find("Player");
		audioSource = GameObject.Find("SongsSelection/Audio Source");
		originalScale = new Vector3(.65f, .65f, .65f);
		loadingScreen = GameObject.Find("Player/MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
	{

		//Debug.Log(transform.position + gameObject.name);
		transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, (((-transform.position.z + 3) / 6))/.7f);
		if (transform.position.z >2.9f)
		{
			Title.text = song.title;
			Description.text = song.description;
			if (song.audioClip != null)
				audioSource.GetComponent<AudioSource>().clip = song.audioClip;
			if (audioSource.GetComponent<AudioSource>().isPlaying == false)
				audioSource.GetComponent<AudioSource>().Play();
			
		}
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && transform.position.z < 2.9f)
		{
			if (transform.position.x < 0)
			{
				transform.parent.GetComponent<SelectSongsComponent>().SwitchSongRight();
			}
			else if(transform.position.x >0)
			{
				transform.parent.GetComponent<SelectSongsComponent>().SwitchSongLeft();
			}
		}

		if (Input.GetMouseButtonDown(0) && transform.position.z >2.9f)
		{
			StartCoroutine(LaunchSong(song));
			loadingScreen.GetComponent<SceneSwitch>().LoadScene();

		}

	}


	IEnumerator LaunchSong(SongScriptableObject song)
	{

		bool _waiting = true;
		float _timer = 0;
		while (_waiting)
		{
			if (_timer > 1.5)
			{
				platform.transform.position += Vector3.up * Time.deltaTime * 3;
				player.transform.position += Vector3.up * Time.deltaTime * 3;
			}

			GetComponent<Renderer>().material.SetFloat("Vector1_798353CA", Mathf.Lerp(_timer - 1, 1, _timer / 5));

			if (_timer > 7)
				_waiting = false;

			_timer += Time.deltaTime;

			yield return 0;

		}



		
		yield break;
	}
}
