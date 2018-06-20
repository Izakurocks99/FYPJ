using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatColor
{
    Red,
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
            ControllerScript controller = other.gameObject.GetComponentInParent<ControllerScript>();
            PlayerStats player = playerCam.GetComponent<PlayerStats>();
            switch (color)
            {
                case BeatColor.Red:
                    {
                        if (controller.controllerColor == ControllerColor.Red)
                        {
                            controller.VibrateController();
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
                        if (controller.controllerColor == ControllerColor.Green)
                        {
                            controller.VibrateController();
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
                        if (controller.controllerColor == ControllerColor.Blue)
                        {
                            controller.VibrateController();
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
                        if (controller.controllerColor == ControllerColor.Gold)
                        {
                            controller.VibrateController();
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
