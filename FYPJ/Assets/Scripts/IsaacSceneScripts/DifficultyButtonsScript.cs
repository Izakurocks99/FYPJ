using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtonsScript : MonoBehaviour
{

    enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD,
    }

    [SerializeField]
    Difficulty difficulty = Difficulty.MEDIUM;

    DifficultyMenuScript menuScript;

    // Use this for initialization
    void Start()
    {
        menuScript = GetComponentInParent<DifficultyMenuScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        menuScript.SelectDifficulty(this);
        PlayerPrefs.SetInt("difficulty", (int)difficulty);
    }

    private void OnCollisionEnter(Collision collision)
    {
        menuScript.SelectDifficulty(this);
        PlayerPrefs.SetInt("difficulty", (int)difficulty);
    }
}
