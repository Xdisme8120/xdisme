using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamaeInfo : MonoBehaviour {

	void Start () {
		if (PhotonNetwork.isMasterClient)
			Invoke ("destory",4.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3 (0, 1, 0)*Time.fixedTime);
	}

	void destory()
	{
		PhotonNetwork.Destroy (this.gameObject);
	}
}
