#define BEAT_POOL
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBandVisualiser : MonoBehaviour {

    public Vector3 beatScale;
    public GameObject[] _goAudioScales;
    public GameObject[] _goPrefab;
    public GameObject _goAudio;
    public GameObject _goPlayer;
#if (BEAT_POOL)
    public Shader DissolveShader = null;
    List<Material> DissolveMaterialPool = null;
    const uint MaterialPoolSize = 6;
    public Texture2D RoughnessBeatTex = null;
    public Texture2D MetallicBeatTex = null;
#endif
    public Material[] _materials;
    public float _ftTime;
    public float _ftWait = 0.5f;
    public float _ftSpeed = 1f;
    float[] _ftAryPrevBuffer;
    float[] _ftAryDiffBuffer;
	public static int _intBeatCounts;
    int _intBeats;
    int _intPreviousMaterial;
    int _intCurrentMaterial;
#if (BEAT_POOL)
    [SerializeField]
    List<GameObject> listGOPrefab = new List<GameObject>();
    List<List<GameObject>> listGOBeatPool = null;
    [Range(50,500)]
    uint _maxBeats = 10;
#endif 
    void Start() {
		_ftAryPrevBuffer = new float[AudioSampler._ftMaxbuffer.Length];
		_ftAryDiffBuffer = new float[AudioSampler._ftMaxbuffer.Length];
		_ftTime = 0f;
		_intBeatCounts = 0;
		_intBeats = 0;
		_intPreviousMaterial = 0;
		_intCurrentMaterial = 10;
#if (BEAT_POOL)
        Debug.Assert(DissolveShader);
        Debug.Assert(RoughnessBeatTex);
        Debug.Assert(MetallicBeatTex);
        DissolveMaterialPool = new List<Material>();
        for (int i = 0; i < MaterialPoolSize; i++)
        {
            Material temp = new Material(DissolveShader);
            temp.SetTexture("_RoughnessTex", RoughnessBeatTex);
            temp.SetTexture("_MetallicTex", MetallicBeatTex);
            temp.SetTexture("NoiseTex", 
                NoiseTexGenerator.GetTexture((float)i, (float)i));
            DissolveMaterialPool.Add(temp);
        }
        //for(int i = 0)
        listGOBeatPool = new List<List<GameObject>>();
        for (int i1 = 0; i1 < listGOPrefab.Count; i1++)
        {
            listGOBeatPool.Add(new List<GameObject>());
            for (int i2 = 0; i2 < _maxBeats; i2++)
            {
                listGOBeatPool[i1].Add( Instantiate(listGOPrefab[i1]));
                listGOBeatPool[i1][i2].SetActive(false);
            }
        }
#endif
    }

	void Update () {
		for (int i = 0; i < _goAudioScales.Length; i++)
			_goAudioScales[i].transform.localScale = new Vector3(1, AudioSampler._ftBandbuffer[i] * 0.5f + 1, 1);

		switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty)
		{
			case 0: {
				_ftWait = 1.25f;
				break;
			}
			case 1: {
				_ftWait = 0.975f;
				break;
			}
			case 2: {
				_ftWait = 0.85f;
				break;
			}
			case 3: {
				_ftWait = 0.65f;
				break;
			}
			default: {
				_ftWait = 1.5f;
				break;
			}
		}
		
		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
		{
			GetDifference();
			InstantiateBeat();
		}
	}

	void InstantiateBeat() {
		if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 0) {
			if ((_ftTime += 1 * Time.deltaTime * _ftSpeed) >= _ftWait) {

				if (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty != 0) {
					float _ftSpawn = Random.value;
					int _intMax = (_ftSpawn < 0.6f) ? 2 : 1;
                    int _intStore = 0;
                    for (int i = _goAudioScales.Length - 1; i >= 0; i--) {
						if (_intBeats < _intMax) {

							for (int k = 0; k < 2; k++) {
								if ((AudioSampler._ftMaxbuffer[i] - _ftAryPrevBuffer[i]) == _ftAryDiffBuffer[k]) {
                                    if (_intStore == i)
                                        continue;
                                    else
                                        _intStore = i;

									_intCurrentMaterial = Random.Range(0, _materials.Length);
									if (_intCurrentMaterial == _intPreviousMaterial) {

										if (_intPreviousMaterial != 0)
											_intCurrentMaterial -= 1;
										else if (_intPreviousMaterial == 0)
												 _intCurrentMaterial = _materials.Length - 1;
									}
									if (_intCurrentMaterial == _intPreviousMaterial - 2 ||
										_intCurrentMaterial == _intPreviousMaterial + 2) {

										if (_intCurrentMaterial != 0)
											_intCurrentMaterial -= 1;
										else if (_intCurrentMaterial == 0)
												 _intCurrentMaterial = _materials.Length - 1;
									}

#if (BEAT_POOL)
                                    GameObject go = GetObjectFromPool(_intCurrentMaterial, _goAudioScales[i]);
                                   
                                    //_beatPool.PopBack();
                                    //go.transform.parent = _goAudioScales[i].transform.parent.transform;
                                    //go.transform.position = _goAudioScales[i].transform.parent.position;
#else
                                     GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[i].transform.parent.transform, false);
#endif 
                                    go.transform.localScale = beatScale;
									go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
									_intPreviousMaterial = _intCurrentMaterial;

									go.name = "Beat " + i;
									go.SetActive(true);
									_intBeats++;
#if (BEAT_POOL)
                                    InitPoolObject(go, _intCurrentMaterial);
#endif
                                }	}	}	}	}

				for (int j = _goAudioScales.Length - 1; j >= 0; j--) {
					if (_intBeats < 2) {
					
						if (AudioSampler._ftMaxbuffer[j] == AudioSampler._ftMaxbuffer.Max()) {

							_intCurrentMaterial = Random.Range(0, _materials.Length);
							if (_intCurrentMaterial == _intPreviousMaterial) {

								if (_intPreviousMaterial != 0)
									_intCurrentMaterial -= 1;
								else if (_intPreviousMaterial == 0)
										 _intCurrentMaterial = _materials.Length - 1;
							}
							if (_intCurrentMaterial == _intPreviousMaterial - 2 ||
								_intCurrentMaterial == _intPreviousMaterial + 2) {

								if (_intCurrentMaterial != 0)
									_intCurrentMaterial -= 1;
								else if (_intCurrentMaterial == 0)
										 _intCurrentMaterial = _materials.Length - 1;
							}

#if (BEAT_POOL)
                            GameObject go = GetObjectFromPool(_intCurrentMaterial, _goAudioScales[j]);
                            //GameObject go = 
                            //_beatPool.PopBack();
                            //go.transform.parent = _goAudioScales[j].transform.parent.transform;
                            //go.transform.position = _goAudioScales[j].transform.parent.position;
                            //InitPoolObject(go,_goAudioScales[j].transform.parent.transform); 
#else
                            GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[j].transform.parent.transform, false);
#endif
                            go.transform.localScale = beatScale;
							go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
							_intPreviousMaterial = _intCurrentMaterial;

							go.name = "Beat " + j;
							go.SetActive(true);
							_intBeats++;
#if (BEAT_POOL)
                             InitPoolObject(go, _intCurrentMaterial);
#endif
				}	}	}

				_intBeats = 0;
				_ftTime = 0;
	}	}	}
#if (BEAT_POOL)
    GameObject GetObjectFromPool(int index, GameObject parent)
    {
        GameObject go = listGOBeatPool[index].PopBack();
        go.transform.parent = parent.transform.parent.transform;
        go.transform.position = parent.transform.parent.position;
        return go;
    }
    void InitPoolObject(GameObject go, int poolsindex)
    {
        go.SetActive(true);
        //go.transform.position = new Vector3(0, 0, 0);
        go.GetComponent<AudioMotion>().PoolInit(listGOBeatPool[poolsindex],DissolveMaterialPool); // responsible for returing the object
        go.GetComponent<BulletScript>().PoolInit();
    }
#endif

    void GetDifference() {
		for (int i = 0; i < _ftAryDiffBuffer.Length; i++) {

			float _ftDifference = AudioSampler._ftMaxbuffer[i] - _ftAryPrevBuffer[i];
			if (_ftDifference < 0)
				_ftDifference *= -1;
			_ftAryDiffBuffer[i] = _ftDifference;
		}

		_ftAryDiffBuffer = _ftAryDiffBuffer.OrderByDescending(ft => ft).ToArray();
	}
}
