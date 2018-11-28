using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyUiControl : MonoBehaviour {

	private static LobbyUiControl _instace;
	public static LobbyUiControl Instance
	{
		get{return _instace;}
	}
	public GameObject StartPanel;
	public GameObject LoginPanel;
	public GameObject RegisertPanel;
	public GameObject MainPanel;
	public GameObject ConfigPanel;
	public GameObject SoundPanel;
	public GameObject EffectPanel;

	public Text Message;
	public GameObject MessageBox;


	public MenuAnimator Ma;

	void Awake()
	{
		_instace = this;
	}



	public void StartGame()
	{
		ChangePanel (LoginPanel, StartPanel);
		Ma.StartGame ();
	}

	public void FinishLOgin()
	{
		ChangePanel (MainPanel,LoginPanel);
		Ma.FinishLogin ();
	}

	public void GoRegister()
	{
		ChangePanel (RegisertPanel,LoginPanel);
		Ma.GoRegister ();
	}

	public void FinishRegister()
	{
		ChangePanel (RegisertPanel, LoginPanel);
		Ma.FinishRegister ();
	}

	public void LogOut()
	{
		ChangePanel (LoginPanel,MainPanel);
		Ma.LoginOut ();
	}

	public void ShowMessage(string Message)
	{
		MessageBox.SetActive (true);
		this.Message.text = Message;
	}


	void ChangePanel(GameObject Open,GameObject Close)//开启关闭Panel的函数
	{
		Open.SetActive (true);
		Close.SetActive (false);
	}


}
