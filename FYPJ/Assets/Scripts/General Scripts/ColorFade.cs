using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour {

	public GameObject _goAudio;
	public Color _color0;
	public Color _color1;
	public Color _color2;
	public Color _color3;
	Color _color;
	Renderer _renderer;
	public float _ftWait;
	float _ftTime;
	int _intPoint;

	void Start () {
		if (_goAudio == null)
			_goAudio = GameObject.Find("AudioSampler");
		_renderer = this.GetComponent<Renderer>();
		_ftTime = 0.0f;
		_intPoint = 0;
	}
	
	void Update () {
		if (_goAudio.GetComponent<AudioSource>().isPlaying == true &&
			_goAudio.GetComponent<AudioSource>().clip != null) {

			_renderer.material.color = Color.Lerp(_color0, _color1, _ftTime);

			if (_ftTime < 1.0f)
				_ftTime += Time.deltaTime / _ftWait;
			else
				_ftTime = 0;
		}
	}
}
