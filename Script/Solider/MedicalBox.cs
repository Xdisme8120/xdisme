using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalBox : MonoBehaviour {


	private PhotonView pv;

	void Start()
	{
		Invoke ("destory",10.0f);
		pv = GetComponent<PhotonView> ();
	}
	void OnTriggerEnter(Collider player)
	{
		if (player.gameObject.tag == "Player") {
			player.GetComponent<PhotonView> ().RPC ("GetHealth", PhotonTargets.All,50);
			destory ();
		}
	}

	void destory()
	{
		if(pv.isMine)
			PhotonNetwork.Destroy (this.gameObject);
	}
}
