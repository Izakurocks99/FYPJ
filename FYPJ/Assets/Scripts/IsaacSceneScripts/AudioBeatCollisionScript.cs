#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeatVars
{
    public GameColors color;
    public Material material;
}


public class AudioBeatCollisionScript : MonoBehaviour
{
    // Use this for initialization
    public float lifeTime = 10f;
    public PlayerStats playerCam;
    public List<BeatVars> beats;
    public bool isHit = false;
    GameColors color = GameColors.NONE;
    Dictionary<Material, GameColors> dicBeat;
    Rigidbody rb;

    public float life;

    
#if (BEAT_POOL)
    public void PoolInit()
    {
        life = lifeTime;
        this.enabled = true;
        isHit = false;
        gameObject.GetComponent<Collider>().enabled = true;
#else
    void Start()
    {
#endif
        playerCam = FindObjectOfType<PlayerStats>();

        if (dicBeat == null)
        {
            dicBeat = new Dictionary<Material, GameColors>();
            foreach (BeatVars beat in beats)
            {
                dicBeat.Add(beat.material, beat.color);
            }
        }

        if (color == GameColors.NONE)
            color = dicBeat[gameObject.GetComponent<Renderer>().sharedMaterial];

        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        //Debug.Log(GetComponent<Renderer>().material.GetTexture("_EmissionMap"));
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            this.GetComponent<AudioMotion>().Die();

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

                //Destroy(transform.gameObject);
                //if(gameObject.GetComponent<AudioMotion>())
                gameObject.GetComponent<AudioMotion>().Die();

                this.enabled = false;
                isHit = true;
                //rb.useGravity = true;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
#if (BEAT_POOL)
    void OnReturn()
#else
    void OnDestroy()
#endif
    {

    }
}
