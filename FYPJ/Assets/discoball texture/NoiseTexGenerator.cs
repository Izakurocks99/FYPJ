using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseTexGenerator {

    // Use this for initialization
    static uint widht = 800;
    static uint height = 600;

    static float Scale = 30f;

    static byte[] NoiseData = null;
    
	public static Texture2D GetTexture (float xoff,float yoff) {
        //Renderer rend = this.GetComponent<Renderer>() as Renderer;

        Texture2D Noise = null; 
        Noise = new Texture2D((int)widht, (int)height, TextureFormat.Alpha8, false);
        if(NoiseData == null)
            NoiseData = new byte[height * widht];
        // y x
        for (uint y = 0; y < height; y++)
        {
            for (uint x = 0; x < widht; x++)
            {
                NoiseData[widht * y  + x] =  (byte) (255 * Mathf.Clamp(
                    Mathf.PerlinNoise((xoff + x / (float)widht) * Scale,
                    (yoff + y / (float)height) * Scale) , 0.1f, 1f));
                //NoiseData[widht * y + x] = (byte)Mathf.Clamp(
                 //   Mathf.PerlinNoise((x / (float)widht) * Scale,
                  //  (y / (float)height) * Scale) , 0f, 1f);
            }
        }
        Noise.LoadRawTextureData(NoiseData);
        Noise.Apply();
        //        rend.material.SetTexture("NoiseTex", Noise);
        //       NoiseData = null;
        return Noise;
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
// gen x textures and give the textures for objects randomly
