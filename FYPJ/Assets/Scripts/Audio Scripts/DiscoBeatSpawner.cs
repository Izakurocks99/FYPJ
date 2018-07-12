using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBeatSpawner : MonoBehaviour {

	public GameObject _goPrefab;
	public GameObject _goPlayer;
	public GameObject _goAudio;
	public GameObject _goBandVisualiser;
	float _ftTime;
	float _ftWait;
	float _ftXIncrement;
	float _ftY;
	int _intCount;
	int _intCurrent;
	bool _blFlip;

    //ObjectPooling
    public Shader dissolveShader;
    List<Material> dissolveMaterialPool;
    List<GameObject> discobeatPool;
    List<GameObject> trashBin;

	void Start()
    {
        dissolveMaterialPool = new List<Material>();
        discobeatPool = new List<GameObject>();
        trashBin = new List<GameObject>();

        _ftTime = 0.0f;
		_ftWait = 0.25f;
		_ftXIncrement = 0.0f;
		_intCount = 0;
		_intCurrent = 0;
		_blFlip = false;

        //if there is objects in the pool
        for (int i = 0; i < 25; i++)
        {
            GameObject go;
            //create a new GO
            go = Instantiate(_goPrefab, this.transform, false);
            discobeatPool.Add(go);
            go.SetActive(false);
            Debug.Assert(dissolveShader);
            Material temp = new Material(dissolveShader);
            Material goMat = go.GetComponent<Renderer>().material;

            temp.SetTexture("_MainTex", goMat.GetTexture("_MainTex"));
            temp.SetTexture("_Emissive", goMat.GetTexture("_EmissionMap"));
            temp.SetTexture("_RoughnessTex", goMat.GetTexture("_SpecGlossMap"));
            temp.SetTexture("_MetallicTex", goMat.GetTexture("_MetallicGlossMap"));

            temp.SetTexture("NoiseTex",
                NoiseTexGenerator.GetTexture(dissolveMaterialPool.Count, dissolveMaterialPool.Count));
            dissolveMaterialPool.Add(temp);
            //go.transform.position

        }
    }

	void Update () {
		if (_intCount == 0)
			_intCount = Random.Range(21, 27);

			_ftY = Mathf.Lerp(-0.25f, 0.25f, Mathf.PingPong(Time.time, 1));

        if (_goAudio.GetComponent<AudioSource>().clip != null &&
            _goAudio.GetComponent<AudioSource>().isPlaying == true &&
           (_goAudio.GetComponent<AudioSource>().time < _goAudio.GetComponent<AudioSource>().clip.length * 0.95f))
        {
            if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 1)
            {
                GenerateDrag();
            }
            else
            {
                if(trashBin.Count>0)
                { 
                    for (int i = 0; i < trashBin.Count; ++i)
                    {
                        discobeatPool.Add(trashBin[i]);
                    }
                    //discobeatPool = trashBin;
                    trashBin.Clear();
                }
            }
        }
	}

	void GenerateDrag() {
		Vector3 _vec3View = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

		if (_intCount % 2 == 1)
			_ftXIncrement = 0.5f / (_intCount / 2);
		else if (_intCount % 2 == 0)
			_ftXIncrement = 0.5f / (((float)_intCount / 2.0f) + 0.5f);

		if (_blFlip == true)
			if (_ftXIncrement < 0)
				_ftXIncrement = -_ftXIncrement;
		else
			if (_ftXIncrement > 0)
				_ftXIncrement = -_ftXIncrement;

		if ((_ftTime += 1 * Time.deltaTime) >= _ftWait) {
			if (_intCurrent < _intCount) {

                //GameObject go = Instantiate(_goPrefab, this.transform, false);
                GameObject go = InitPoolObject(this.transform);

                go.name = "Test " + _intCurrent;
				go.SetActive(true);

				go.transform.position = this.GetComponent<DiscoMotion>()._vec3Spawn;
				float _ftX = _ftXIncrement * _intCurrent;

				if (_blFlip == true)
					go.GetComponent<DiscoBeatMotion>()._vec3Area = new Vector3(0.5f, 0.0f, 0.0f);
				else
					go.GetComponent<DiscoBeatMotion>()._vec3Area = new Vector3(-0.5f, 0.0f, 0.0f);

				go.GetComponent<DiscoBeatMotion>()._vec3Shift = new Vector3(_ftX, _ftY, 0);
				go.GetComponent<DiscoBeatMotion>()._vec3Target = _vec3View;
				
				_intCurrent += 1;
				// _goBandVisualiser.GetComponent<AudioBandVisualiser>()._ftTime -= 0.1f;
			}
				
			else {

				_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint = 0;
				_intCurrent = 0;
				float _ftFlip = Random.value;
				if (_ftFlip < 0.5f)
					_blFlip = !_blFlip;
				else
					return;
			}

			_ftTime = 0.0f;
		}
	}

    GameObject InitPoolObject(Transform parent)
    {
        GameObject go;
        if(discobeatPool.Count == 0)
        {
            go = Instantiate(_goPrefab, this.transform, false);
            discobeatPool.Add(go);
            go.SetActive(false);

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

        go = discobeatPool.PopBack();
        go.transform.parent = parent;
        go.transform.position = parent.transform.position;

        if (go)
            go.SetActive(true);
        //go.transform.position = new Vector3(0, 0, 0);
        go.GetComponent<DiscoBeatMotion>().PoolInit(trashBin, dissolveMaterialPool); // responsible for returing the object
        go.GetComponent<DiscoBeatCollisionScript>().PoolInit();
        return go;
    }
}
