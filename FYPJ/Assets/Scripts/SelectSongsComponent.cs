﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSongsComponent : MonoBehaviour
{

    public GameObject prefabs;
    public SongScriptableObject[] songs;
    public Text Title;
    public Text Description;
    public GameObject platform;
    public GameObject audioSource;
    public GameObject player;
    public SceneSwitch loadingScreen;

    [SerializeField]
    CDFollower follower;
    Rigidbody _rigidbody;

    public bool selected = true;
    Quaternion tempRot;
    Vector3 tempForward;

    public CDscript currCD;

    public float distance;
    public float minDist;
    public float selectDist;

    // Use this for initialization
    void Start()
    {
        //Getting Values
        loadingScreen = FindObjectOfType<SceneSwitch>();
        player = FindObjectOfType<PlayerControlsScript>().gameObject;

        //songsSelector = gameObject;
        tempRot = follower.transform.rotation;
        tempForward = follower.transform.forward;


        _rigidbody = GetComponent<Rigidbody>();

        for (int i = 0; i < songs.Length; i++)
        {
            GameObject _instanceSong = Instantiate(prefabs);
            _instanceSong.transform.position = this.transform.position;
            _instanceSong.transform.parent = this.transform;
            _instanceSong.name = "Song" + i;
            _instanceSong.transform.position += Vector3.forward * distance;
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, (i + 1) * (360 / (songs.Length)), this.transform.rotation.z);

            CDscript cd = _instanceSong.GetComponent<CDscript>();
            cd.Title = Title;
            cd.Description = Description;
            cd.audioSource = audioSource;
            cd.song = songs[i];
            cd.frontPoint = this.transform.position + Vector3.forward * distance;
            cd.minDist = minDist;
            cd.maxDist = distance;
            cd.selectDist = selectDist;
            cd.parent = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && selected)
        {
            ToggleSelected();
        }
    }

    private void FixedUpdate()
    {
        if (selected)
        {
            float angleInDegrees;
            //angleInDegrees = Quaternion.Angle(tempRot, follower.transform.rotation);
            angleInDegrees = Vector3.SignedAngle(tempForward, follower.transform.forward, transform.up);
            Vector3 angularDisplacement = transform.up * angleInDegrees * Mathf.Deg2Rad;
            Vector3 angularSpeed = tempRot * (angularDisplacement / Time.deltaTime);
            tempRot = follower.transform.rotation;
            tempForward = follower.transform.forward;
            //Quaternion destination = follower.transform.rotation;

            //Vector3 angularSpeed = transform.rotation * (angularDisplacement / Time.deltaTime);

            _rigidbody.angularVelocity = angularSpeed;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !selected)
        {
            ToggleSelected();
            tempRot = follower.transform.rotation;
            tempForward = follower.transform.forward;
        }
    }

    bool ToggleSelected()
    {
        return selected = !selected;
    }

    public void LaunchSong()
    {
        StartCoroutine(currCD.LaunchSong());
        PlayerPrefs.SetString("test", "test");
    }
    //public void SwitchSongRight()
    //{
    //    if (clickable)
    //    {
    //        clickable = false;
    //        _startRotation = songsSelector.transform.eulerAngles;
    //        _toRotation = new Vector3(0, songsSelector.transform.eulerAngles.y + 360 / songs.Length, 0);
    //        StartCoroutine("SwitchRight");
    //    }
    //}

    //IEnumerator SwitchRight()
    //{
    //    count = 0;
    //    while (count < 1.15)
    //    {
    //        songsSelector.transform.eulerAngles = Vector3.Lerp(_startRotation, _toRotation, count);
    //        count += Time.deltaTime;
    //        yield return 0;
    //    }
    //    clickable = true;
    //    yield break;
    //}

    //public void SwitchSongLeft()
    //{
    //    if (clickable)
    //    {
    //        clickable = false;
    //        _startRotation = songsSelector.transform.eulerAngles;
    //        _toRotation = new Vector3(0, songsSelector.transform.eulerAngles.y - 360 / songs.Length, 0);
    //        StartCoroutine("SwitchRight");
    //    }
    //}
}