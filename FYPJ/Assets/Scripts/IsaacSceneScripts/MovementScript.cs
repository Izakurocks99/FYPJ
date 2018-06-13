using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class MovementScript : MonoBehaviour
{
    //Movement mode
    //Cannot pick up items
    //Select the ground and point a dir to move and face the direction


    public bool isMoveController = false;
    public bool isSecondaryMoveController = false;
    public GameObject player;
    public GameObject movementMarker;
    public LaserPointer laserPointer;
    private RaycastHit hit;
    private bool hasMoveInput = false;
    private bool isMovePressed = false;
    private Vector3 startingPos;

    // Use this for initialization
    void Start ()
    {
        startingPos = player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        hasMoveInput = CheckForInput();

        if (hasMoveInput && !isMovePressed)
        {
            isMovePressed = true;
            //create icon
            if (!movementMarker.activeInHierarchy)
                SpawnMarker();

            //interact with UI
            if (laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
            
        }
        else if(!hasMoveInput && isMovePressed)
        {
            isMovePressed = false;
            //move player and remove icon
            Move();

        }

        //update marker to controller forward
        if (movementMarker && movementMarker.activeInHierarchy && isMovePressed)
        {
            movementMarker.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }

        //reset player position
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            //player.transform.position = startingPos;
        }
    }

    // Move controlelrs use an API for their analog button, DualShock 4 uses an axis name for R2
    bool CheckForInput()
    {
#if UNITY_PS4
        if (isMoveController)
        {
            if (!isSecondaryMoveController)
            {
                return (PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
            }
            else
            {
                return (PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
            }
        }
        else
        {
            return (Input.GetAxisRaw("TriggerRight") > 0 ? true : false);
        }
#else
		return Input.GetButton("Fire1");
#endif
    }

    //Move to Location
    void Move()
    {
        if (movementMarker.activeInHierarchy)
        {
            player.transform.position = movementMarker.transform.position;//move player
            
            player.transform.forward = movementMarker.transform.forward; 
            movementMarker.SetActive(false);
        }
    }

    void SpawnMarker()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (CheckRayHitGround(hit)) //hits te ground
            {
                movementMarker.SetActive(true); // create marker
                Vector3 height = new Vector3(0f, 0.5f, 0f); //set marker height
                movementMarker.transform.position = hit.point + height;
            }
        }
    }

    // When fired the controller will vibrate for 0.1 seconds
    IEnumerator Vibrate()
    {
#if UNITY_PS4
        if (isMoveController)
        {
            if (isSecondaryMoveController)
                PS4Input.MoveSetVibration(0, 1, 128);
            else
                PS4Input.MoveSetVibration(0, 0, 128);
        }
        else
        {
            PS4Input.PadSetVibration(0, 0, 255);
        }
#endif

        yield return new WaitForSeconds(0.1f);

#if UNITY_PS4
        if (isMoveController)
        {
            if (isSecondaryMoveController)
                PS4Input.MoveSetVibration(0, 1, 0);
            else
                PS4Input.MoveSetVibration(0, 0, 0);
        }
        else
        {
            PS4Input.PadSetVibration(0, 0, 0);
        }
#endif
    }

    bool CheckRayHitGround(RaycastHit hit)
    {
        return hit.transform.gameObject.tag == "Ground";
    }
}
