#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBeatCollisionScript : MonoBehaviour
{
    // Use this for initialization
    public float lifeTime = 10f;
    public PlayerStats playerCam;
    public bool isHit = false;
    GameColors color = GameColors.Rainbow;
    Rigidbody rb;

    public float life;
    bool isCollided = false;
    SoundEffectsScript soundEffects;

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
        soundEffects = FindObjectOfType<SoundEffectsScript>();
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
                this.GetComponent<DiscoBeatMotion>().Die();

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
                    if (stick.heldController)
                        stick.heldController.VibrateController();
                    player.ModifyScore(Mathf.RoundToInt(rb.velocity.magnitude + 1));
                    playerCam.ModifyCombo(true);
                    soundEffects.PlaySound("RainbowHit");
                    //addscore
                }
                else
                {
                    //lowerscore
                    playerCam.ModifyCombo(false);
                    soundEffects.PlaySound("MissBeat");
                }
                
                    gameObject.GetComponent<DiscoBeatMotion>().Die();

                life = 0.3f;
                isHit = true;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
