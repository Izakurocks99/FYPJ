using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class WeaponController : MonoBehaviour
{
	public float fireRate = 0.5f;
	public ParticleSystem shotEmitter;
	public bool isMoveController = false;
	public bool isSecondaryMoveController = false;
    public AudioSource shotSound;
    public AudioSource missSound;
    public GameObject player;
    //private LaserPointer laserPointer;
	private RaycastHit hit;
	private float lastShotTime = 0;
	private bool canShoot = true;
	private bool hasShootInput = false;
	[HideInInspector]
	public int shotsFired = 0;

	// Used for initialization
	void Start ()
	{
		//laserPointer = FindObjectOfType<LaserPointer>();
		WeaponCanShoot(false);
		shotsFired = 0;
	}

	// Get input from DualShock 4 or Move controller(s), then shoot if possible
	void Update ()
	{
		hasShootInput = CheckForInput();

		if(canShoot && hasShootInput && Time.time > lastShotTime + fireRate)
		{
			Fire();
		}
	}

	// Move controlelrs use an API for their analog button, DualShock 4 uses an axis name for R2
	bool CheckForInput()
	{
#if UNITY_PS4
		if(isMoveController)
		{
			if(!isSecondaryMoveController)
			{
				return(PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
			}
			else
            {
                return (PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
			}
		}
		else
		{
			return(Input.GetAxisRaw("TriggerRight") > 0 ? true : false);
		}
#else
		return Input.GetButton("Fire1");
#endif
	}

	// Fire the weapon, including a particle and audio effect
	void Fire()
	{
        StartCoroutine(Vibrate());

		lastShotTime = Time.time;
		shotEmitter.Emit(1);
		shotSound.Play();
		shotsFired++;

        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{
        //          if (hit.transform.GetComponentInParent<TargetObject>())
        //              hit.transform.GetComponentInParent<TargetObject>().DestroyTarget();
        //          else
        //              missSound.Play();
        //}
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                Vector3 height = new Vector3(0f, 1.5f, 0f);
                player.transform.position = hit.point + height;
                Debug.Log(hit.collider.gameObject.name);
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

    // Public call for toggling the weapon state
    public void WeaponCanShoot(bool shootBool)
	{
		//canShoot = shootBool;
		//laserPointer.gameObject.SetActive(shootBool);
	}
}
