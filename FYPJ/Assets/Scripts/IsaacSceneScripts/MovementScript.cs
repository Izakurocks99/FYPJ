using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class MovementScript : ControllerModesScript
{
    //Movement mode
    //Cannot pick up items
    //Select the ground and point a dir to move and face the direction

        
    public GameObject player;
    public GameObject movementMarker;
    private RaycastHit hit;
    private Vector3 startingPos;

    private void Awake()
    {
        startingPos = player.transform.position;
    }

    // Update is called once per frame
    void Update() {
        //update marker to controller forward
        if (movementMarker && movementMarker.activeInHierarchy)
        {
            movementMarker.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }
    }

    public override void ButtonPressed(ControllerButtons button, bool isSecondaryController)
    {
        if (button == controlsScript.interactButton)
        {
            SpawnMarker();
        }
        else if(button == controlsScript.resetPosButton)
        {
            ResetPlayerPos();
        }
        else if (button == controlsScript.resetSceneButton)
        {
            ResetScene();
        }
    }

    public override void ButtonReleased(ControllerButtons button, bool isSecondaryController)
    {
        if(button == controlsScript.interactButton)
        {
            Move();
        }
    }

    //Move to Location
    void Move()
    {
        //move to marker
        if (movementMarker.activeInHierarchy)
        {
            player.transform.position = movementMarker.transform.position;//move player

            player.transform.forward = movementMarker.transform.forward;
            movementMarker.SetActive(false);
        }
    }

    void SpawnMarker()
    {
        //spawn the marker
        if (Physics.Raycast(transform.position, transform.forward, out hit) && !movementMarker.activeInHierarchy)
        {
            if (CheckRayHitGround(hit)) //hits te ground
            {
                movementMarker.SetActive(true); // create marker
                Vector3 height = new Vector3(0f, 0.5f, 0f); //set marker height
                movementMarker.transform.position = hit.point + height;
            }
        }
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void ResetPlayerPos()
    {
        player.transform.position = startingPos;
    }

    bool CheckRayHitGround(RaycastHit hit)
    {
        return hit.transform.gameObject.tag == "Ground";
    }

    public override void Init()
    {
        enabled = true;
    }
}
