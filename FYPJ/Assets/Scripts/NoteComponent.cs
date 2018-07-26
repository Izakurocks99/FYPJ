using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteComponent : MonoBehaviour {

	ObjectPool pool;
	public float limitDistance = 0.1f;
	public int number;
	float count = 0;
	public int typeIndex;
	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public Vector3 destination;
	private float travelTime = 2;

    [SerializeField]
    GameColors color = GameColors.NONE;
    public bool isHit = false;
    Rigidbody rb;

    // Use this for initialization
    void OnEnable ()
	{
		this.GetComponent<SphereCollider>().enabled = true;
		this.transform.GetChild(1).GetComponent<Transform>().localScale = Vector3.one * 0.75f;		//reset particles localscales
		this.transform.GetChild(2).GetComponent<Transform>().localScale = Vector3.one * 0.75f;
		this.GetComponent<Renderer>().material.SetFloat("_TreshHold", 0);							//reset material  dissolve threshold to 0
		pool = GameObject.FindObjectOfType<ObjectPool>();											//find the pool in the scene
		count = 0;
		travelTime = GameObject.Find("MusicSource").GetComponent<MusicDelay>().delay;				//make the travel time fit song delay
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        isHit = false;
    }


	
	// Update is called once per frame
	void Update ()
	{
        if (!isHit)
        {
            this.gameObject.transform.Rotate(this.transform.up, 4.0f);                                  //beat rotating on its own axis
            this.transform.position = Vector3.Lerp(startPos, destination, count);                       //beat moving towards the target
            count += Time.deltaTime * (1 / travelTime);
            if (Vector3.Distance(this.transform.position, destination) < .5f)               //if beat touching the target, the player miss and it disappear.
            {
                StartCoroutine("Dissolve");                                                             //dissolving it
            }
        }
	}


	IEnumerator Dissolve()
	{
		float _currentThreshold = 0;																//initial dissolve threshold
		Vector3 _currentThresholdVector = Vector3.one*.75f;                                         //initial particle local scale

		//Make the beat fade
		while (_currentThreshold < 1)																
		{
			_currentThreshold += Time.deltaTime;
			this.GetComponent<Renderer>().material.SetFloat("_TreshHold", Mathf.Lerp(0,.4f,_currentThreshold));													//dissolve threshold goes up
			this.transform.GetChild(1).GetComponent<Transform>().localScale = Vector3.Lerp(_currentThresholdVector, Vector3.zero, _currentThreshold);			//particles scale goes down
			this.transform.GetChild(2).GetComponent<Transform>().localScale = Vector3.Lerp(_currentThresholdVector, Vector3.zero, _currentThreshold);
			yield return null;																																	//Wait next frame and continue the loop
		}


		transform.parent = pool.transform;															//resetting its parent and position
		transform.localPosition = new Vector3(0, 0, 0);
		pool.ReturnObjectToPool(this.gameObject, typeIndex);                                        //desactivate it and return it to the pool
		yield break;
	}


	private void OnCollisionEnter(Collision collision)
	{
        if (!isHit)
        {
            if (collision.gameObject.GetComponentInParent<BatonCapsuleFollower>())
            {
                BatonCapsuleFollower follower = collision.gameObject.GetComponentInParent<BatonCapsuleFollower>();
                PlayerStickScript stick = follower._batonFollower.thisStick;
                PlayerStats player = stick.gameObject.transform.root.GetComponentInChildren<PlayerStats>();

                if (stick.currColor == color || color == GameColors.Rainbow)
                {
                    if (stick.heldController)
                        stick.heldController.VibrateController();
                    player.ModifyScore(Mathf.RoundToInt(rb.velocity.magnitude));
                    player.ModifyCombo(true);
                    //addscore
                }
                else
                {
                    //lowerscore
                    player.ModifyCombo(false);
                }

                StartCoroutine("Dissolve");                                                             //dissolving it

                this.GetComponent<SphereCollider>().enabled = false;
                isHit = true;
                //gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

}
