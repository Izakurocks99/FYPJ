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
    [SerializeField]
    Texture2D tex3 = null;

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


    //[SerializeField]
    //float curve = 0.5f;

    [SerializeField]
    float curveRange = 180f;

    float GetHeight(int x)
    {
        float height = 0;
        if (x < w / 2)
        {
           // height = Mathf.Lerp(0, curve, (float)x / ((float)w / 2f));
        }
        else
        {
            x = w - x;
           // height = Mathf.Lerp(0, curve, (float)x  / ((float)w / 2f));
        }

        return height;
    }

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
        Vector3[] normals = new Vector3[w * h];

        //float midpoint = (float)w / 2f;
        float singleDegree =  curveRange / (float)w;

        //float distAdd = 180f / curveRange;
        float circleArea = w * dist * (360f / curveRange);



        float r = circleArea / (2f * Mathf.PI);//Mathf.Sqrt(circleArea / Mathf.PI);

        //todo lerp just the z value of the curve
        // and the the normal can calculate froms neighbours

       // Color[] cols = new Color[w * h];
       Vector2 axis = new Vector2(-r,0);
        float startAngle = 90 - curveRange / 2;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float angle = singleDegree * x + startAngle;

                float rcos = Mathf.Cos(Mathf.Deg2Rad * angle); 
                float rsin = Mathf.Sin(Mathf.Deg2Rad * angle);

                //float height = GetHeight(x);//x < w / 2 ? Mathf.Lerp(0, curve, w * (dist / 2) / x * (dist / 2)) : Mathf.Lerp(curve, 0 , w * (dist / 2) / x * (dist / 2));


                Vector2 ans = new Vector2(rcos * (float)axis.x + rsin* (float)axis.y, -rsin * (float)axis.x + rcos * (float)axis.y);
                //ans = new Vector2(midpoint , 0) + ans;

                verts[w * y + x] = new Vector3(ans.x , y * dist, ans.y - r);// ans.y * dist );
                Vector2 norm = ans.normalized * -1f;
                normals[w * y + x] = new Vector3(norm.x, 0, norm.y);
                uvs[w * y + x] = new Vector2((float)x / (float)w, ((float)y / (float)h) / 10.0f);
                inds[w * y + x] = i++;
            }
        }
        points.vertices = verts;
        // points.colors = cols;
        points.normals = normals;
        points.uv = uvs;
        
        points.SetIndices(inds,MeshTopology.Points,0);
        points.bounds = new Bounds(Vector3.zero, 1000f * Vector3.one);

        m = new Material(sha);
        m.SetTexture("_Texture1", tex);
        m.SetTexture("_Texture2", tex1);
        m.SetTexture("_Texture3", tex2);
        m.SetTexture("_Texture4", tex3);

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
