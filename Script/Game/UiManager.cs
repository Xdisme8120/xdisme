using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiManager : MonoBehaviour {

	public Button[] BronBtn;
	public Button[] SelectTeam;
	public bool IsBron;
	public AudioListener Al;
	public PhotonView pv;
	public static UiManager _inastanc;
	public Image[] Res;
	public static UiManager Instance
	{
		get{return _inastanc;}

	}
	void Awake()
	{	
		_inastanc = this;
	}

	void Start () {
		Al = GetComponent<AudioListener> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ShoWTeamSelect()
	{
		foreach (Button b in SelectTeam)
			b.gameObject.SetActive (true);
	}

	public void SetTeam(int team)
	{
		Bron.Instance.Team = team;
		foreach (Button b in SelectTeam)
			b.gameObject.SetActive (false);
		ShowSelectPlayer();
	}
	public void ShowSelectPlayer()
	{
		if (Bron.Instance.Team == 0) {
			PhotonNetwork.player.SetTeam (PunTeams.Team.blue);
			BronBtn [0].gameObject.SetActive (true);
			BronBtn [1].gameObject.SetActive (true);
			BronBtn [2].gameObject.SetActive (true);
		} else {
			PhotonNetwork.player.SetTeam (PunTeams.Team.red);
			BronBtn [3].gameObject.SetActive (true);
			BronBtn [4].gameObject.SetActive (true);
			BronBtn [5].gameObject.SetActive (true);	
		}

	}

	public void SetSolider(int soliderType)
	{
		Bron.Instance.soliderType = soliderType;
		if (IsBron == true) 
		{
			return;
		}
			else 
		{
			Bron.Instance.BronPlayer ();
			IsBron = true;
		}
	}

	public void GameOver(int team)
	{
		if (Bron.Instance.Team != team)
			Res [0].enabled = true;
		else
			Res [1].enabled = true;
	}

	/*public void setBronBtn()
	{
		if (IsBron) {
			Al.enabled = false;
			if (Bron.Instance.Team == 0) {
				BronBtn [0].gameObject.SetActive (false);
				BronBtn [1].gameObject.SetActive (false);
				BronBtn [2].gameObject.SetActive (false);
			} else {
				BronBtn [3].gameObject.SetActive (false);
				BronBtn [4].gameObject.SetActive (false);
				BronBtn [5].gameObject.SetActive (false);	
			}
			IsBron = false;
		} else {
			Al.enabled = true;
			if (Bron.Instance.Team == 0) {
				BronBtn [0].gameObject.SetActive (true);
				BronBtn [1].gameObject.SetActive (true);
				BronBtn [2].gameObject.SetActive (true);
			} else {
				BronBtn [3].gameObject.SetActive (true);
				BronBtn [4].gameObject.SetActive (true);
				BronBtn [5].gameObject.SetActive (true);	
			}
			IsBron = true;
		}
	}*/
}
