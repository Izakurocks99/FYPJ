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

    public float playerHeight;    
    public GameObject player;
    public GameObject movementMarker;
    public Transform stickSlot;
    private RaycastHit hit;
    private Vector3 startingPos;
    private GameObject highlightedStick;
    private ControllerButtons pressedButton;
    private bool canPickup = true;
    

    private void Awake()
    {
        startingPos = player.transform.position;
    }

    // Update is called once per frame
    void Update() {
        //update marker to controller forward
        if (movementMarker && movementMarker.activeInHierarchy && pressedButton == controlsScript.interactButton)
        {
            movementMarker.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }
    }

    public override void ButtonPressed(ControllerButtons button, bool isSecondaryController)
    {
        pressedButton = button;
        if (button == controlsScript.interactButton)
        {
            if (highlightedStick && canPickup)//if overlapping a stick
            {
                //put stick in the hand's stick slot
                highlightedStick.transform.parent = stickSlot;
                highlightedStick.transform.position = stickSlot.position;
                highlightedStick.transform.localScale = stickSlot.localScale;
                highlightedStick.transform.rotation = stickSlot.rotation;
                canPickup = false;
            }
            else
                //set highlighted stick to null
                //else
                //spawn a marker
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
        pressedButton = ControllerButtons.NONE;
        if (button == controlsScript.interactButton)
        {
            //move the player
            Move();
        }
    }

    //Move to Location
    void Move()
    {
        //move to marker
        if (movementMarker.activeInHierarchy)
        {
            Vector3 height = new Vector3(0f, playerHeight, 0f); //set marker height
            player.transform.position = movementMarker.transform.position + height;//move player

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

    private void OnTriggerEnter(Collider other)
    {
        //if other entered a sicks
        if (other.gameObject.tag == "PlayerStick")
        {
            //set current stick as "highlighted"
            highlightedStick = other.gameObject;
            Debug.Log(highlightedStick.name);
        }
        if (other.gameObject.GetComponent<PlayerStickScript>())
        {
            PlayerStickScript stickScript = other.gameObject.GetComponent<PlayerStickScript>();
            stickScript.handle.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if other entered a sicks
        if (other.gameObject.tag == "PlayerStick")
        {
            //set current stick as "highlighted"
            highlightedStick = null;
        }
        if (other.gameObject.GetComponent<PlayerStickScript>())
        {
            PlayerStickScript stickScript = other.gameObject.GetComponent<PlayerStickScript>();
            stickScript.handle.SetActive(false);
        }

    }
}
