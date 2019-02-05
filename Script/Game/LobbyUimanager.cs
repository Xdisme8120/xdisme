using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public Text nickname;
    public Text level;
    public Text exp;
    public Text gold;

	void Awake()
	{
		_instance = this;

	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshInfo()
    {
        string path = Application.persistentDataPath + "/user.json";
        StreamReader sr = new StreamReader(path);
        string jsonStr = sr.ReadToEnd();
        JsonData json = JsonMapper.ToObject(jsonStr);
        //        Debug.Log("level:" + json["data"]["uLevel"].ToString());
        //        Debug.Log("jsonStr:"+jsonStr);
        PlayerPrefs.SetString("account", json["data"]["username"].ToString());
        if (json["data"]["uNickname"] != null)
        {
            nickname.text = json["data"]["uNickname"].ToString();
            PlayerPrefs.SetString("nickname", nickname.text);
        }
        if(json["data"]["uLevel"] != null)
        level.text = json["data"]["uLevel"].ToString();
        if (json["data"]["uExp"] != null)
            exp.text = json["data"]["uExp"].ToString();
        if (json["data"]["uGold"] != null)
            gold.text = json["data"]["uGold"].ToString();
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
