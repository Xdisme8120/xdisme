using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDestory : MonoBehaviour {

	public float DestoryTime;
	void Start () {
		Invoke ("Des", DestoryTime);
	}
	

	void Des()
	{
		if (GetComponent<PhotonView> ().isMine) {
			PhotonNetwork.Destroy (this.gameObject);
		}
	}
}
