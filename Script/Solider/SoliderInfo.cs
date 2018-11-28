using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoliderInfo : MonoBehaviour ,IPunObservable{

	public int Health;
	public float MoveSpeed;
	public Slider HealthSlider;
	public ETCJoystick Etcj;
	public ETCButton Fire;
	public ETCButton GetV;
	public ETCButton Skill;
	public ETCBase ChangeW;
	public PhotonView pv;
	public int OwnerId;
	public Shot shotCompoment;
	public bool IsOnV = false;
	public  CameraFllow CF;
	public AnimatorMW Amw;
	private VechielController VC;
	public bool Team;
	public GameObject DeadBody;
	public PhotonView pointPv;
	void Start () {
		CF = GameObject.Find ("Cam").GetComponent<CameraFllow> ();
		VC = GetComponent<VechielController> ();
		Amw = GetComponent<AnimatorMW> ();
		pv = GetComponent<PhotonView> ();
		OwnerId = pv.ownerId; 
		shotCompoment = GetComponent<Shot> ();
		IniInfo ();

	}
	
	// Update is called once per frame
	void Update () {
		HealthSlider.value = Health;


	}

	void IniInfo()
	{
		Etcj.tmSpeed = MoveSpeed;
		Fire.onDown.AddListener (() => shotCompoment.Fire ());
		Fire.onUp.AddListener (() => shotCompoment.StopFire ());
		GetV.onDown.AddListener (() => VC.VechielAction());
		if(Skill.gameObject!=null)
		Skill.onDown.AddListener (() => shotCompoment.useSkill ());
	}

	void OnTriggerEnter(Collider Collider)
	{
		if (IsOnV == false)
		{
			if (Collider.tag == "Vechiel") 
			{
				GetV.gameObject.SetActive (true);
			}
		}
	}
		

	[PunRPC]
	void GetDamage(int _damage,int ViewId)
	{
		Health -= _damage;
	
		if (Health <= 0) 
		{
			if (pv.isMine) 
			{
				PhotonView.Find (ViewId).RPC ("GetKill", PhotonTargets.All, null);
				PhotonNetwork.player.AddScore (1);
				CF.SetCurrentTragert (CF.InitGameObject);

			}
				
			gameObject.tag="Untagged";
			die ();
		}
	}

	[PunRPC]
	void GetHealth(int Health)
	{
		this.Health += Health;
		this.Health = Mathf.Clamp (this.Health, 0, 100);
		
	}

	[PunRPC]
	void GetKill()
	{
		Debug.Log ("KillSomeBody");
	}

	public void die()
	{
		if (pv.isMine) 
		{
			UiManager.Instance.Al.enabled = true;
			Debug.Log (PhotonNetwork.player.GetScore ());
			CF.CurrentTraget = CF.InitGameObject;
			UiManager.Instance.IsBron = false;
			Bron.Instance.BronPlayer ();
			PhotonNetwork.Instantiate (DeadBody.name, transform.position, transform.rotation, 0);
			destory ();
		}

	}

	void destory()
	{
		PhotonNetwork.Destroy (this.gameObject);
	}

	void OnTriggerStay(Collider V)
	{
		if (pv.ownerId == PhotonNetwork.player.ID) {
			if (V.gameObject.tag == "Vechiel" && V.gameObject.GetComponent<Vechiel> ().HavePeople == false) {
				Debug.Log ("candeiver");
				GetV.gameObject.SetActive (true);
				VC.nearVechiel = V.gameObject;
			} else
				return;
		}

	}

	void OnTriggerExit(Collider V)
	{

		if (pv.isMine) {
			if (!IsOnV) {
				if (V.gameObject.tag == "Vechiel") {
					GetV.gameObject.SetActive (false);
					VC.nearVechiel = null;
				}
			}
		}
	}


	public  void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
			stream.SendNext (Health);
		else
			Health = (int)stream.ReceiveNext ();
	}
}
