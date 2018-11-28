using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PunData : MonoBehaviour,IPunObservable {

	public string Name;
	public Text NameShow;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		NameShow.text = Name;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
			stream.SendNext (Name);
		else
			Name = (string)stream.ReceiveNext();
	}
}
