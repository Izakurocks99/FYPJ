using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	[CreateAssetMenu(fileName = "new Song" , menuName = "Song")]
public class SongScriptableObject : ScriptableObject {

	public string title;
	public string description;
	public AudioClip audioClip;
	public int songNumber;


}
