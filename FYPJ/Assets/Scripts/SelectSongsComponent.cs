using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSongsComponent : MonoBehaviour {

    public GameObject prefabs;
	public GameObject songsSelector;
	public SongScriptableObject[] songs;
	Vector3 _startRotation;
	Vector3 _toRotation;
	float count = 0;
	bool clickable = true;

	public float distance = 2.5f, rotationSpeed = 5 ,angularStrength = 1;

    // Use this for initialization
    void Start()
    {
		songsSelector = gameObject;

        for (int i = 0; i < songs.Length; i++)
        {
            GameObject _instanceSong = Instantiate(prefabs);
            _instanceSong.transform.position = this.transform.position;
            _instanceSong.transform.parent = this.transform;
            _instanceSong.name = "Song" + i;
            _instanceSong.transform.position += Vector3.forward*distance;
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, (i+1)*(360 / (songs.Length)), this.transform.rotation.z);

			_instanceSong.GetComponent<CDscript>().song = songs[i];

        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		//transform.Rotate(Vector3.up * Time.deltaTime* rotationSpeed, Space.Self);


	}

	public void SwitchSongRight()
	{
		if (clickable)
		{
			clickable = false;
			_startRotation = songsSelector.transform.eulerAngles;
			_toRotation = new Vector3(0, songsSelector.transform.eulerAngles.y + 360 / songs.Length, 0);
			StartCoroutine("SwitchRight");
		}
	}

	IEnumerator SwitchRight()
	{
		count = 0;
		while (count < 1.15)
		{
			songsSelector.transform.eulerAngles = Vector3.Lerp(_startRotation, _toRotation, count);
			count += Time.deltaTime;
			yield return 0;
		}
		clickable = true;
		yield break;
	}

	public void SwitchSongLeft()
	{
		if (clickable)
		{
			clickable = false;
			_startRotation = songsSelector.transform.eulerAngles;
			_toRotation = new Vector3(0, songsSelector.transform.eulerAngles.y - 360 / songs.Length, 0);
			StartCoroutine("SwitchRight");
		}
	}
}
