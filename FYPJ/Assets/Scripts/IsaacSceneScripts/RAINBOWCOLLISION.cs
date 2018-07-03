#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAINBOWCOLLISION : MonoBehaviour
{
    // Use this for initialization
    public float lifeTime = 10f;
    public PlayerStats playerCam;
    public bool isHit = false;
    GameColors color = GameColors.Rainbow;
    Rigidbody rb;

    public float life;
    bool isCollided = false;

    void Start()
    {
        playerCam = FindObjectOfType<PlayerStats>();
        isCollided = false;
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
            if (gameObject.GetComponent<AudioMotion>())
                this.GetComponent<AudioMotion>().Die();

            this.enabled = false;
            if (!isHit)
                playerCam.ModifyCombo(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isCollided)
        {
            isCollided = true;
            if (other.gameObject.GetComponentInParent<BatonCapsuleFollower>())
            {
                BatonCapsuleFollower follower = other.gameObject.GetComponentInParent<BatonCapsuleFollower>();
                PlayerStickScript stick = follower._batonFollower.thisStick;
                PlayerStats player = playerCam.GetComponent<PlayerStats>();

                if (stick.currColor == color || color == GameColors.Rainbow)
                {
                    stick.heldController.VibrateController();
                    player.ModifyScore(1);
                    playerCam.ModifyCombo(true);
                    //addscore
                }
                else
                {
                    //lowerscore
                    player.ModifyScore(-10);
                    playerCam.ModifyCombo(false);
                }

                //Destroy(transform.gameObject);
                if(gameObject.GetComponent<AudioMotion>())
                    gameObject.GetComponent<AudioMotion>().Die();

                life = 1;
                gameObject.GetComponent<DiscoBeatMotion>().enabled = false;
                isHit = true;
                rb.useGravity = true;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
