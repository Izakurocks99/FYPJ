using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBeatSpawner : MonoBehaviour {

	public GameObject[] _goContainer;
	public GameObject[] _goPrefab;
	public Vector3 _vec3Scale;

    public Shader dissolveShader = null;
    List<Material> dissolveMaterialPool = null;
    //List<GameObject> listGOPrefab = new List<GameObject>();
    List<List<GameObject>> listGOBeatPool = null;

    private void Start()
    {
        dissolveMaterialPool = new List<Material>();
        listGOBeatPool = new List<List<GameObject>>();
        for (int i1 = 0; i1 < _goPrefab.Length; i1++)
        {
            listGOBeatPool.Add(new List<GameObject>());
            for (int i2 = 0; i2 < 10; i2++)
            {
                GameObject go;
                go = Instantiate(_goPrefab[i1], this.transform, false);
                listGOBeatPool[i1].Add(go);
                listGOBeatPool[i1][i2].SetActive(false);

                Material temp = new Material(dissolveShader);
                Material goMat = go.GetComponent<Renderer>().material;

                temp.SetTexture("_MainTex", goMat.GetTexture("_MainTex"));
                temp.SetTexture("_Emissive", goMat.GetTexture("_EmissionMap"));
                temp.SetTexture("_RoughnessTex", goMat.GetTexture("_SpecGlossMap"));
                temp.SetTexture("_MetallicTex", goMat.GetTexture("_MetallicGlossMap"));

                temp.SetTexture("NoiseTex",
                    NoiseTexGenerator.GetTexture(dissolveMaterialPool.Count, dissolveMaterialPool.Count));
                dissolveMaterialPool.Add(temp);

            }
        }
    }


    void Update () {
		this.transform.parent.transform.LookAt(Camera.main.transform);
	}

	public void SpawnSpeakerBeat() {
		int _intRandomContainer = Random.Range(0, _goContainer.Length);
		float _ftRandomPrefab = Random.value;

		if (_ftRandomPrefab < 0.6f) {
            //GameObject go = Instantiate(_goPrefab[0], _goContainer[_intRandomContainer].transform.parent.transform.parent.transform, false);

            GameObject go = InitPoolObject(0, _goContainer[_intRandomContainer].transform.parent.transform.parent.transform);


            go.name = "Test";
			go.SetActive(true);
		}
		
		else {
			//GameObject go = Instantiate(_goPrefab[1], _goContainer[_intRandomContainer].transform.parent.transform.parent.transform, false);
            GameObject go = InitPoolObject(0, _goContainer[_intRandomContainer].transform.parent.transform.parent.transform);
            go.transform.localScale = _vec3Scale;
			
			go.name = "Test";
			go.SetActive(true);
		}
	}

    GameObject InitPoolObject(int index, Transform parent)
    {
        GameObject go;
        //if there is objects in the pool
        if (listGOBeatPool[index].Count > 0)
        {
            go = listGOBeatPool[index].PopBack();
            go.transform.parent = parent;
            go.transform.position = parent.transform.position;

            //go.transform.position
        }
        else
        {
            //create a new GO
            go = Instantiate(_goPrefab[index], this.transform, false);

            Material temp = new Material(dissolveShader);
            Material goMat = go.GetComponent<Renderer>().material;

            temp.SetTexture("_MainTex", goMat.GetTexture("_MainTex"));
            temp.SetTexture("_Emissive", goMat.GetTexture("_EmissionMap"));
            temp.SetTexture("_RoughnessTex", goMat.GetTexture("_SpecGlossMap"));
            temp.SetTexture("_MetallicTex", goMat.GetTexture("_MetallicGlossMap"));

            temp.SetTexture("NoiseTex",
                NoiseTexGenerator.GetTexture(dissolveMaterialPool.Count, dissolveMaterialPool.Count));
            dissolveMaterialPool.Add(temp);
        }

        go.SetActive(true);
        //go.transform.position = new Vector3(0, 0, 0);
        go.GetComponent<SpeakerBeatMotion>().PoolInit(listGOBeatPool[index], dissolveMaterialPool); // responsible for returing the object
        go.GetComponent<SpeakerBeatCollisionScript>().PoolInit();
        return go;
    }
}
