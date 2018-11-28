using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	//public AudioSource As;
	public AudioClip[] ACs;

	void Start () {
		
	}
	
	[PunRPC]
	public void ShotSound()
	{
		AudioSource.PlayClipAtPoint (ACs [0], transform.position);
	}
	[PunRPC]
	public void RPGSound()
	{
		AudioSource.PlayClipAtPoint (ACs [1], transform.position);
	}
}
