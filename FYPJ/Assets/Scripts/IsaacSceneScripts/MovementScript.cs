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
    //Movement mode:
    //Picks up sticks
    //Moves player
    //TODO:
    //see if i can move some codes into MovementMarkerScript
    //a cancel movement button
    //fix the player facing direction after moving

    public bool autoRot = true;

    public float playerHeight; // to set the height of the player after moving cann line cast to the ground to determine height instead
    public GameObject player; //to move the player can switch to transform instead

    public GameObject movementMarker; //to move the marker, and set it active/inactive
    private MovementMarkerScript markerScript; //to lock the other controller from modifying the marker

    private RaycastHit hit; // to get the point on the ground the player wants to move
    private Vector3 startingPos; // for the player to reset the position
    private ControllerScript controller; //to get which controller this script is


    private void Awake()
    {
        startingPos = player.transform.position;
        markerScript = movementMarker.GetComponent<MovementMarkerScript>();
        controller = gameObject.GetComponent<ControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //update marker to controller forward
        if (movementMarker && movementMarker.activeInHierarchy && markerScript.controlledBySecondary == controller.isSecondaryMoveController)
        {
            movementMarker.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }
    }

    public override void ButtonPressed(ControllerButtons button)
    {
        if (button == controlsScript.interactButton)
        {
            //spawn a movement marker
            SpawnMarker();

        }
    }

    public override void ButtonReleased(ControllerButtons button)
    {
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
        if (controller.laserPointer && Physics.Raycast(transform.position, transform.forward, out hit) && !movementMarker.activeInHierarchy)
        {
            if (CheckRayHitGround(hit)) //hits te ground
            {
                movementMarker.SetActive(true); // create marker
                Vector3 height = new Vector3(0f, 0.5f, 0f); //set marker height
                movementMarker.transform.position = hit.point + height;
                markerScript.controlledBySecondary = controller.isSecondaryMoveController;
            }
        }
    }

    bool CheckRayHitGround(RaycastHit hit)
    {
        return hit.transform.gameObject.tag == "Ground";
    }

    public override void Init()
    {
    }

}
