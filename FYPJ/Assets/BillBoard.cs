using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtension
{
    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }
    public static T PopRandom<T>(this IList<T> list)
    {
        return list.PopAt(Random.Range(0, list.Count - 1));
    }
}

public class BillBoard : MonoBehaviour {

    [SerializeField]
    GameObject playerstats = null;
    Material m = null;
    PlayerStats stats = null;
    void Start(){
        m = this.GetComponent<Renderer>().material;
        Debug.Assert(m);
        m.SetFloat("firstDigit",0);
        m.SetFloat("lastDigit",0);
        m.SetFloat("secondDigit",0);
        m.SetFloat("thirdDigit",0);

        Debug.Assert(playerstats);
        stats = playerstats.GetComponent<PlayerStats>();
	}

    // Update is called once per frame

    int GetFirstDigit(string num)//int number)
    {
        return num.Length >= 4 ? int.Parse(num[0].ToString()) : 0;
    }
    int GetLastDigit(string num)//int num)
    {
        return int.Parse(num[num.Length - 1].ToString());
    }
    int GetSecondDigit(string num)//int num)
    {
        if(num.Length  == 3)
        {
            return int.Parse(num[0].ToString());
        }
        else if(num.Length == 4)
        {
            return int.Parse(num[1].ToString());
        }
        return 0;
        //return   num.Length >= 2 ? int.Parse(num[ num.Length >= 3 ? 1 : 0].ToString()) : 0;
    }
    int GetThirdDigit(string num)//int num)
    {
        if(num.Length == 2)
        {
            return int.Parse(num[0].ToString());
        }
        else if(num.Length == 3)
        {
            return int.Parse(num[1].ToString());
        }
        else if(num.Length == 4)
        {
            return int.Parse(num[2].ToString());
        }
        return 0;
        //return   num.Length >= 2 ? int.Parse(num[ num.Length >= 3 ? 1 : 0].ToString()) : 0;
    }

    [SerializeField]
    int num = 0;
    void Update () 
    {
        num = stats._intPlayerScoring;
        if (num < 0) return;
        List<GameColors> d = new List<GameColors>();
        string n = num.ToString();
        m.SetFloat("firstDigit", GetFirstDigit(n));
        m.SetFloat("secondDigit", GetSecondDigit(n));
        m.SetFloat("thirdDigit", GetThirdDigit(n));
        m.SetFloat("lastDigit", GetLastDigit(n));
	}
}
