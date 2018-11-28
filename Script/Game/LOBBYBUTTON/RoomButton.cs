using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomButton : MonoBehaviour {

	public enum RoomStates
	{
		HavePlayer,
		NoPlayer,
		none
	};
	public string RoomName;
	public Text ButtonText;
	public RoomStates roomstates=RoomStates.none;
	private RoomOptions option;
	void Start () {
		
	}
		
	public void setRoomState(RoomStates state)
	{
		switch (state) 
		{
		case RoomStates.HavePlayer:
			ButtonText.text = "加入游戏";
			roomstates = state;
				break;
		case RoomStates.NoPlayer:
			roomstates = state;
			ButtonText.text = "开始游戏";
				break;
		}
	}

	public void StartGame()
	{

		PhotonNetwork.JoinOrCreateRoom (RoomName,option,TypedLobby.Default);
	
	}

}
