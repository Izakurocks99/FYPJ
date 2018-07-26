using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour {

	public GameObject _goAudio;
	public Color _color0;
	public Color _color1;
	public Color _color2;
	public Color _color3;
	Color _colorA;
	Color _colorB;
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

			switch (_intPoint) {
				case 0: {
						_colorA = _color0;
						_colorB = _color1;
					}
				break;
				case 1: {
						_colorA = _color1;
						_colorB = _color2;
				}
				break;
				case 2: {
						_colorA = _color2;
						_colorB = _color3;
				}
				break;
				case 3: {
						_colorA = _color3;
						_colorB = _color0;
				}
				break;
			}

			_renderer.material.color = Color.Lerp(_colorA, _colorB, _ftTime);

			if (_ftTime < 1.0f)
				_ftTime += Time.deltaTime / _ftWait;
			else {
				if (_intPoint + 1 < 4)
					_intPoint += 1;
				else
					_intPoint = 0;
				_ftTime = 0;
			}
		}
	}
}
