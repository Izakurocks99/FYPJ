using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MyExtension
{
    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }

}




public class BillBoard : MonoBehaviour {

    [SerializeField]
    Shader sha = null;
    // Use this for initialization
    [SerializeField]
    int w = 0, h = 20;

    [SerializeField]
    float dist = 0.5f;

    [SerializeField]
    Texture2D tex = null;
    [SerializeField]
    Texture2D tex1 = null;
    [SerializeField]
    Texture2D tex2 = null;

    Material m = null;

    //[SerializeField]
    //List<Color> colors = new List<Color>();
    [SerializeField]
    Color col1 = new Color();
    //[ColorUsageAttribute(false,true)]
    [SerializeField]
    Color col2 = new Color();
    //[ColorUsageAttribute(false,true)]

    [SerializeField]
    float scale1 = 1;
    [SerializeField]
    float scale2 = 1;

    void Start () {
        Debug.Assert(sha);
        Debug.Assert(tex);
        Debug.Assert(tex1);
        Debug.Assert(tex2);
       // Debug.Assert(colors.Count != 0);
        Mesh points = null;//new Mesh();
        GetComponent<MeshFilter>().mesh = points = new Mesh();
        Vector3[] verts = new Vector3[w * h];
        Vector2[] uvs = new Vector2[w * h];
        int[] inds = new int[w * h];
        int i = 0;
       // Color[] cols = new Color[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                verts[w * y + x] = new Vector3(x * dist, y * dist, 0);
                uvs[w * y + x] = new Vector2((float)x / (float)w, ((float)y / (float)h) / 10.0f);
                inds[w * y + x] = i++;
              //  cols[w * y + x] = colors.GetRandom();
            }
        }
        points.vertices = verts;
       // points.colors = cols;
        points.uv = uvs;
        
        points.SetIndices(inds,MeshTopology.Points,0);
        points.bounds = new Bounds(Vector3.zero, 1000f * Vector3.one);

        m = new Material(sha);
        m.SetTexture("_Texture1", tex);
        m.SetTexture("_Texture2", tex1);
        m.SetTexture("_Texture3", tex2);

        m.SetColor("_Color1",col1);
        m.SetColor("_Color2",col2);

        m.SetFloat("_Scale1", scale1);
        m.SetFloat("_Scale2", scale1);
        this.GetComponent<Renderer>().material = m;

        //this.GetComponent<>
	}

    // Update is called once per frame

    int GetFirstDigitLoop(string num)//int number)
    {
        return num.Length >= 3 ? int.Parse(num[0].ToString()) : 0;
        //while (number >= 10)
       // {
        //    number = (number - (number % 10)) / 10;
       // }
       // return number;
    }
    int GetLastDigit(string num)//int num)
    {
        return int.Parse(num[num.Length - 1].ToString());
        //return num % 10;
    }
    int GetMiddleDigit(string num)//int num)
    {
        return   num.Length >= 2 ? int.Parse(num[ num.Length >= 3 ? 1 : 0].ToString()) : 0;
    }
    [SerializeField]
    int num = 666;
    void Update () {
        m.SetFloat("_Scale1", scale1);
        m.SetFloat("_Scale2", scale2);

        m.SetColor("_Color1", col1);
        m.SetColor("_Color2", col2);
        if (num < 0) return;
        string n = num.ToString();
        m.SetFloat("firstDigit", GetFirstDigitLoop(n));
        m.SetFloat("midDigit", GetMiddleDigit(n));
        m.SetFloat("lastDigit", GetLastDigit(n));
		
	}
}
