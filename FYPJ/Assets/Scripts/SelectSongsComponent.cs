using System.Collections;
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
    public GameObject difficultyMenu;
    public SceneSwitch loadingScreen;

    [SerializeField]
    public CDFollower follower = null;
    [SerializeField]
    public Vector3 frontPoint;
    [SerializeField]
    GameObject debugObj = null;
    Rigidbody _rigidbody;

    public bool selected = true;
    Quaternion tempRot;
    Vector3 tempForward;
    Vector3 startForward;

    public CDscript currCD;

    public float distance;
    public float minDist;
    public float selectDist;

    public float slowdownMultiplyer = 0.5f;

    private GameObject playerCam;
    [SerializeField]
    Vector3 playerOffset = new Vector3();

    bool snap = true;


    // Use this for initialization
    void Start()
    {
        startForward = transform.forward;
        //Getting Values
        loadingScreen = FindObjectOfType<SceneSwitch>();
        player = FindObjectOfType<PlayerControlsScript>().gameObject;
        playerCam = player.GetComponentInChildren<Camera>().gameObject;

        //songsSelector = gameObject;
        tempRot = follower.transform.rotation;
        tempForward = follower.transform.forward;


        _rigidbody = GetComponent<Rigidbody>();

        for (int i = 0; i < songs.Length; i++)
        {
            GameObject _instanceSong = Instantiate(prefabs);
            _instanceSong.transform.position = this.transform.position + Vector3.forward * distance;
            _instanceSong.transform.parent = this.transform;
            _instanceSong.name = "Song" + i;
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, (i + 1) * (360 / (songs.Length)), this.transform.rotation.z);

            CDscript cd = _instanceSong.GetComponentInChildren<CDscript>();
            cd.Title = Title;
            cd.Description = Description;
            cd.audioSource = audioSource;
            cd.song = songs[i];
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

        difficultyMenu.transform.position = gameObject.transform.position;
    }

    private void FixedUpdate()
    {

        transform.position = playerCam.transform.position - playerOffset;
        frontPoint = transform.position + Vector3.forward * distance;
        follower.transform.position = transform.position;

        if (debugObj)
            debugObj.transform.position = frontPoint;

        if (_rigidbody.angularVelocity.sqrMagnitude <= 0.5*0.5)
            snap = true;

        if (selected)
        {
            float angleInDegrees;
            //Vector3 followerforward = new Vector3((follower.transform.forward).x, 0, (follower.transform.forward).z).normalized;

            //angle between
            angleInDegrees = Vector3.SignedAngle(tempForward, follower.transform.forward, transform.up);
            //displacement
            Vector3 angularDisplacement = transform.up * angleInDegrees * Mathf.Deg2Rad;
            //speed from current to destination
            Vector3 angularSpeed = tempRot * (angularDisplacement / Time.deltaTime);
            tempRot = follower.transform.rotation;
            tempForward = follower.transform.forward;
            //assign speed
            _rigidbody.angularVelocity = angularSpeed;
        }
        else if(snap)
        {
            //rotate to curr dist to front pos
            float angleInDegrees;
            Vector3 forwardCD = (currCD.transform.position - transform.position).normalized;

            angleInDegrees = Vector3.SignedAngle(forwardCD, startForward, transform.up);
            Vector3 angularDisplacement = transform.up * angleInDegrees * Mathf.Deg2Rad;

            Vector3 angularSpeed = Quaternion.LookRotation(forwardCD) * (angularDisplacement / Time.deltaTime);

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

    public void Select()
    {
        if (!selected)
        {
            ToggleSelected();
            tempRot = follower.transform.rotation;
            tempForward = follower.transform.forward;
        }
    }

    public void Release()
    {
        if (selected)
        {
            ToggleSelected();
        }
    }

    bool ToggleSelected()
    {
        snap = !selected;
        return selected = !selected;
    }

    public void LaunchSong()
    {
        //StartCoroutine(currCD.LaunchSong());
        PlayerPrefs.SetString("test", currCD.song.title);
        difficultyMenu.SetActive(true);
    }

    [SerializeField]
    bool debugSpawningStats;

    bool dual;
    public void ToggleNumColors( bool isdual)
    {
        dual = isdual;
        if (debugSpawningStats)
            Debug.Log("dualcolor" + dual);

        if (dual)
            PlayerPrefs.SetInt("dualcolor", 1);
        else
            PlayerPrefs.SetInt("dualcolor", 0);
    }

    bool random;
    public void ToggleRandomArea(bool isrand)
    {
        random = isrand;
        if (debugSpawningStats)
            Debug.Log("randomarea" + random);

        if (random)
            PlayerPrefs.SetInt("randomarea", 0);
        else
            PlayerPrefs.SetInt("randomarea", 1);
    }
}
