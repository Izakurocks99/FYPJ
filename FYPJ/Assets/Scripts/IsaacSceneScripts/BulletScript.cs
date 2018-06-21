using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatColor
{
    Pink,
    Green,
    Blue,
    Gold,
}

[System.Serializable]
public class BeatVars
{
    public BeatColor color;
    public Material material;
}


public class BulletScript : MonoBehaviour
{

    //public float bulletSpeed = 5f;
    //public float bulletLifeTime = 10f;
    // Use this for initialization
    [SerializeField]
    public PlayerStats playerCam;
    public List<BeatVars> beats;
    BeatColor color;
    Dictionary<Material, BeatColor> dicBeat;

    void Start()
    {
        playerCam = FindObjectOfType<PlayerStats>();
        dicBeat = new Dictionary<Material, BeatColor>();
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
        if (other.gameObject.tag == "PlayerStick")
        {
            PlayerStickScript stick = other.gameObject.GetComponentInParent<PlayerStickScript>();
            PlayerStats player = playerCam.GetComponent<PlayerStats>();
            switch (color)
            {
                case BeatColor.Pink:
                    {
                        if (stick.currColor == ControllerColor.Pink)
                        {
                            stick.heldController.VibrateController();
                            player.ModifyScore(10);
                            //addscore
                        }
                        else
                        {
                            //lowerscore
                            player.ModifyScore(-10);
                        }
                        break;
                    }

                case BeatColor.Green:
                    {
                        if (stick.currColor == ControllerColor.Green)
                        {
                            stick.heldController.VibrateController();
                            player.ModifyScore(10);
                            //addscore
                        }
                        else
                        {
                            //lowerscore
                            player.ModifyScore(-10);
                        }
                        break;
                    }

                case BeatColor.Blue:
                    {
                        if (stick.currColor == ControllerColor.Blue)
                        {
                            stick.heldController.VibrateController();
                            player.ModifyScore(10);
                            //addscore
                        }
                        else
                        {
                            //lowerscore
                            player.ModifyScore(-10);
                        }
                        break;
                    }

                case BeatColor.Gold:
                    {
                        if (stick.currColor == ControllerColor.Gold)
                        {
                            stick.heldController.VibrateController();
                            player.ModifyScore(10);
                            //addscore
                        }
                        else
                        {
                            //lowerscore
                            player.ModifyScore(-10);
                        }
                        break;
                    }
            }
            Destroy(transform.gameObject);
        }
    }
}
