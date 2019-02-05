using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSetUp : MonoBehaviour,IPunObservable {
	
	public Text text;
	public Behaviour[] NeedDisable;
	public GameObject[] NeedDisable2;

	void Start () {
		text.text = PlayerPrefs.GetString("nickname","unnamed");
		if (!GetComponent<PhotonView> ().isMine) 
		{
			foreach (Behaviour B in NeedDisable)
				B.enabled = false;
			foreach (GameObject Go in NeedDisable2)
				Go.SetActive (false);
		
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			
			stream.SendNext (PhotonNetwork.player.NickName);
		}
		else
			text.text = (string)stream.ReceiveNext ();
	}


}
