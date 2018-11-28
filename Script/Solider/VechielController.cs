using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VechielController : MonoBehaviour {

	public GameObject nearVechiel;
	public PhotonView VpV;
	private PhotonView Pv;
	public ETCJoystick Etcj;
	public ETCButton Fire;
	public ETCButton Skill;
	public GameObject Vpoint;
	private AudioListener Al;
	private GameObject CurrentVechiel;
	private SoliderInfo info;
	public Text Ammo;
	public ETCButton[] VechielBtn;
	public ETCButton[] HeadBtn;
	public GameObject VechielUI;
	public GameObject SoliderUI;
	public ETCButton VechielFireButton;
	private Shot shot;
	private int Cur;
	private int Count;
	enum ControllorMode{

		SoliderMode,
		VechielMode
	};

    ControllorMode Controllor;

	void Start () {

		Al = GetComponent<AudioListener> ();
		shot = GetComponent<Shot> ();
		Pv = GetComponent<PhotonView> ();
		Vpoint = GameObject.Find ("Vpoint");
		info = GetComponent <SoliderInfo> ();
		
	}
	
	// Update is called once per frame
	public void VechielAction()
	{
		
		if (!GetComponent<SoliderInfo> ().IsOnV) {
			SwitchController (ControllorMode.VechielMode);
			Al.enabled = false;
			Skill.activated = false;
			Skill.visible = false;
			if (nearVechiel.GetComponent<Vechiel> ().HavePeople)
				return;
			VpV = nearVechiel.GetComponent<PhotonView> ();
			VpV.RPC ("VechielAction", PhotonTargets.All, Pv.viewID);
			gameObject.transform.position = Vpoint.transform.position;
			CurrentVechiel = nearVechiel;
			info.IsOnV = true;
			info.CF.smooth *= 1.6f;
			info.CF.CurrentTraget = nearVechiel.gameObject;
			VechielFireButton.onDown.AddListener (() => nearVechiel.GetComponent<Vshot> ().Fire ());
	

		} else {
			SwitchController (ControllorMode.SoliderMode);
			Al.enabled = true;
			Skill.activated = true;
			Skill.visible = true;
			gameObject.transform.position = CurrentVechiel.transform.position + new Vector3 (0, 3, 0);
			VpV.RPC ("VechielAction", PhotonTargets.All, Pv.viewID);
			info.IsOnV = false;
			info.CF.smooth /= 1.6f;
			info.CF.CurrentTraget = this.gameObject;
			VechielFireButton.onDown.RemoveAllListeners ();
		}
	}

	void Update()
	{
		if (!GetComponent<SoliderInfo> ().IsOnV) {
			Cur = shot.CurrentShellCount;
			Count = shot.ShellCounts;
		} else 
		{
			Cur = CurrentVechiel.GetComponent<Vshot> ().CurrentShell;
			Count = CurrentVechiel.GetComponent<Vshot> ().ShellCounts;
		}
		Ammo.text = Cur + ":/:" + Count;
	}
	void setContorller()
	{
		Etcj.axisX.directTransform = this.gameObject.transform;
		Etcj.axisY.directTransform = this.gameObject.transform;
	}

	void SwitchController(ControllorMode Mode)
	{
		switch (Mode) 
		{
		case ControllorMode.SoliderMode:
			SoliderUI.SetActive (true);
			VechielUI.SetActive (false);
			break;
		case ControllorMode.VechielMode:
			SoliderUI.SetActive (false);
			VechielUI.SetActive (true);
			StartVechielMode();
			break;
		}
	}

	void StartVechielMode()
	{
		foreach (ETCButton btn in VechielBtn) {
			btn.axis.directTransform = nearVechiel.transform;
		}
		foreach (ETCButton btn2 in HeadBtn) {
			btn2.axis.directTransform = nearVechiel.transform.Find ("head").transform;
		}
	}
}
