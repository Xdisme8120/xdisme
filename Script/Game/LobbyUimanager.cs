using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUimanager : MonoBehaviour {

	public Text[] text;
	private RoomInfo[] Rooms;
	public bool[] RoomsExist;
	public static LobbyUimanager _instance;
	public static LobbyUimanager Instance
	{
		get{ return _instance;}
	}

	void Awake()
	{
		_instance = this;

	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetRoomInfo()
	{
		Rooms = PhotonNetwork.GetRoomList ();

		for (int i = 0; i < 3; i++) {
			RoomsExist [i] = false;
			foreach (RoomInfo R in Rooms) 
			{
				
				if (i.ToString() == R.Name) 
				{
					RoomsExist [i] = true; 
				}
			}
		}
		for (int i = 0; i < 3; i++) 
		{
			if(RoomsExist[i]==true)
			{
				foreach (RoomInfo R in Rooms) 
				{
					if (i.ToString () == R.Name)
						text [i].text = R.PlayerCount.ToString () + " / 6";
				}
			}
			else
				text [i].text ="0 / 6";
		}


	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom ("FirestRoom");
	}

}
