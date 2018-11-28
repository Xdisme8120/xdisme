using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bron : MonoBehaviour {

	public static Bron _instance;
	public GameObject[] bronpoint;
	public GameObject[] Player;
	public CameraFllow cam;
	public InputField nameField;
	public Button NameBtn;
	public int Team;
	public int soliderType;
	private bool FirstBron = true;
	private bool IsBroning = false;
	public float BronTime;
	public GameObject DeadPanel;
	public static Bron Instance
	{
		get{return _instance;}
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

	public void BronPlayer()
	{
		if (FirstBron == true) 
		{
			BronColdTime ();
			FirstBron = false;
		}
		else 
		{
			if (IsBroning == false) 
			{
				DeadPanel.SetActive (true);
				Invoke ("BronColdTime", 5.0f);
				IsBroning = true;
			} else
				return;
		}
	}

	void BronColdTime()
	{
		int id = Team;
		int type = soliderType;
		GameObject Bplayer  = PhotonNetwork.Instantiate (Player [type].name, bronpoint [type].transform.position, bronpoint [type].transform.rotation, 0) as GameObject;
		cam.SetCurrentTragert (Bplayer);
		DeadPanel.SetActive (false);
		IsBroning = false;
		UiManager.Instance.IsBron = true;
		GetComponent<AudioListener>().enabled = false;
	}

	public void SetName()
	{
		PhotonNetwork.player.NickName = nameField.text;
		NameBtn.gameObject.SetActive (false);
		UiManager.Instance.ShoWTeamSelect ();
	}
		

}
