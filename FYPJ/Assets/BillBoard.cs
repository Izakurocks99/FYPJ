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

    Material m = null;
    void Start(){
        m = this.GetComponent<Renderer>().material;
        Debug.Assert(m);
        m.SetFloat("firstDigit",0);
        m.SetFloat("lastDigit",0);
        m.SetFloat("midDigit",0);
	}

    // Update is called once per frame

    int GetFirstDigitLoop(string num)//int number)
    {
        return num.Length >= 3 ? int.Parse(num[0].ToString()) : 0;
    }
    int GetLastDigit(string num)//int num)
    {
        return int.Parse(num[num.Length - 1].ToString());
    }
    int GetMiddleDigit(string num)//int num)
    {
        return   num.Length >= 2 ? int.Parse(num[ num.Length >= 3 ? 1 : 0].ToString()) : 0;
    }
    [SerializeField]
    int num = 0;
    void Update () 
    {
        if (num < 0) return;
        string n = num.ToString();
        m.SetFloat("firstDigit", GetFirstDigitLoop(n));
        m.SetFloat("midDigit", GetMiddleDigit(n));
        m.SetFloat("lastDigit", GetLastDigit(n));
	}
}
