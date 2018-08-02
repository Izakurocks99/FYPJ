#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBeatCollisionScript : MonoBehaviour
{
    // Use this for initialization
    public float lifeTime = 10f;
    public PlayerStats playerCam;
    public bool isHit = false;
    [SerializeField]
    GameColors color = new GameColors();
    Rigidbody rb;

    public float life;

    //void Start()
    //{
    //    playerCam = FindObjectOfType<PlayerStats>();
    //    isCollided = false;
    //    life = lifeTime;
    //    this.enabled = true;
    //    isHit = false;
    //    gameObject.GetComponent<Collider>().enabled = true;
    //    rb = gameObject.GetComponent<Rigidbody>();
    //    rb.useGravity = false;
    //    rb.velocity = Vector3.zero;
    //}

    public void PoolInit()
    {
        playerCam = FindObjectOfType<PlayerStats>();
        life = lifeTime;
        this.enabled = true;
        isHit = false;
        gameObject.GetComponent<Collider>().enabled = true;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            this.GetComponent<SpeakerBeatMotion>().Die();

            this.enabled = false;
            if (!isHit)
                playerCam.ModifyCombo(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isHit)
        {
            if (other.gameObject.GetComponentInParent<BatonCapsuleFollower>())
            {
                BatonCapsuleFollower follower = other.gameObject.GetComponentInParent<BatonCapsuleFollower>();
                PlayerStickScript stick = follower._batonFollower.thisStick;
                PlayerStats player = playerCam.GetComponent<PlayerStats>();

                if (stick.currColor == color || color == GameColors.Rainbow)
                {
                    if (stick.heldController)
                        stick.heldController.VibrateController();
                    player.ModifyScore(Mathf.RoundToInt(rb.velocity.magnitude + 1));
                    playerCam.ModifyCombo(true);
                    //addscore
                }
                else
                {
                    //lowerscore
                    playerCam.ModifyCombo(false);
                }
                
                gameObject.GetComponent<SpeakerBeatMotion>().Die();

                this.enabled = false;
                isHit = true;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void Decay()
    {
        if (color == GameColors.Black)
        {
            playerCam.GetComponent<PlayerStats>().ModifyScore(Mathf.RoundToInt(10));
            playerCam.ModifyCombo(true);
        }
        else
        {
            playerCam.ModifyCombo(true);
        }
    }
}
