using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Connect : Photon.PunBehaviour {

	public Text text;
	public Text ping;

	void Awake()
	{
		PhotonNetwork.automaticallySyncScene = true;
	}
	void Start () {

		PhotonNetwork.PhotonServerSettings.AppID = "9a42e9e2-a0a0-4435-8beb-c3b6e46d819f";
		PhotonNetwork.ConnectToRegion (CloudRegionCode.cn, "1");
	
	}
	
	// Update is called once per frame
	void Update () {
		text.text = PhotonNetwork.connectionStateDetailed.ToString ();
		ping.text = PhotonNetwork.GetPing().ToString()+" MS";
	}

	public void createroom()
	{
		PhotonNetwork.CreateRoom ("gsj");
	}

	public void getinroom()
	{
		PhotonNetwork.JoinRoom ("gsj");
	}
	public void GetRoom()
	{
		RoomInfo[] rf= (PhotonNetwork.GetRoomList ());
		foreach (RoomInfo ifr in rf)
			Debug.Log (ifr);
	}
	public void startgame()
	{
		PhotonNetwork.LoadLevel ("Test");
	}

}
