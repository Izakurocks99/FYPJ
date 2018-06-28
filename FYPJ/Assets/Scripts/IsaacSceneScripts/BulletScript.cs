#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum BeatColor
//{
//    Pink,
//    Green,
//    Blue,
//    Gold,
//}

[System.Serializable]
public class BeatVars
{
    public GameColors color;
    public Material material;
}


public class BulletScript : MonoBehaviour
{

    //public float bulletSpeed = 5f;
    //public float bulletLifeTime = 10f;
    // Use this for initialization
    public PlayerStats playerCam;
    public List<BeatVars> beats;
    GameColors color;
    Dictionary<Material, GameColors> dicBeat;

#if (BEAT_POOL)
    public void PoolInit()
    {
        this.enabled = true;
#else
    void Start()
    {
#endif
        playerCam = FindObjectOfType<PlayerStats>();
        dicBeat = new Dictionary<Material, GameColors>();
        foreach (BeatVars beat in beats)
        {
            dicBeat.Add(beat.material, beat.color);
        }
        color = dicBeat[gameObject.GetComponent<Renderer>().sharedMaterial];
        //Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        //bulletLifeTime -= Time.deltaTime;
        //if (bulletLifeTime <=0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerStickScript>())
        {
            PlayerStickScript stick = other.gameObject.GetComponentInParent<PlayerStickScript>();
            PlayerStats player = playerCam.GetComponent<PlayerStats>();
            //switch (color)
            //{
            //    case BeatColor.Pink:
            //        {
            //            if (stick.currColor == GameColors.Pink)
            //            {
            //                stick.heldController.VibrateController();
            //                player.ModifyScore(10);
            //                //addscore
            //            }
            //            else
            //            {
            //                //lowerscore
            //                player.ModifyScore(-10);
            //            }
            //            break;
            //        }

            //    case BeatColor.Green:
            //        {
            //            if (stick.currColor == GameColors.Green)
            //            {
            //                stick.heldController.VibrateController();
            //                player.ModifyScore(10);
            //                //addscore
            //            }
            //            else
            //            {
            //                //lowerscore
            //                player.ModifyScore(-10);
            //            }
            //            break;
            //        }

            //    case BeatColor.Blue:
            //        {
            //            if (stick.currColor == GameColors.Blue)
            //            {
            //                stick.heldController.VibrateController();
            //                player.ModifyScore(10);
            //                //addscore
            //            }
            //            else
            //            {
            //                //lowerscore
            //                player.ModifyScore(-10);
            //            }
            //            break;
            //        }

            //    case BeatColor.Gold:
            //        {
            //            if (stick.currColor == GameColors.Gold)
            //            {
            //                stick.heldController.VibrateController();
            //                player.ModifyScore(10);
            //                //addscore
            //            }
            //            else
            //            {
            //                //lowerscore
            //                player.ModifyScore(-10);
            //            }
            //            break;
            //        }
            //}

            if (stick.currColor == color || color == GameColors.Rainbow)
            {
                //stick.heldController.VibrateController();
                player.ModifyScore(10);
                //addscore
            }
            else
            {
                //lowerscore
                player.ModifyScore(-10);
            }

            //Destroy(transform.gameObject);
            this.GetComponent<AudioMotion>().Die();
            this.enabled = false;
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
