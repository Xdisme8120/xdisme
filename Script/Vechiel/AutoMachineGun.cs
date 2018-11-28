using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMachineGun : MonoBehaviour {


	public int Damage;
	public int Power;
	public float Range;
	public PhotonView Pv;
	public GameObject Shell;
	public Transform ShotPoint;
	public enum Team{
		Blue,
		Red
	};
	public Team team;
	public GameObject Traget;
	private int Mask;
	public bool HaveTraget = false;
	private Vector3 V3look;

	void OnTriggerStay(Collider col)
	{
		if (HaveTraget)
			return;
		if (col.gameObject.layer == Mask) 
		{
			Traget = col.gameObject;
			HaveTraget = true;
			InvokeRepeating ("shot", 0, 0.2f);
		}
	}

	void OnTriggerExit(Collider col)
	{
		HaveTraget = false;
		CancelInvoke ("shot");
	}

	void Start()
	{
		switch(team)
		{
		case Team.Blue:
			Mask = 11;
			break;
		case Team.Red:
			Mask = 10;
			break;
		}
	}
	void Update()
	{
		if (Traget == null) 
		{
			CancelInvoke ("shot");
			HaveTraget = false;

		}
		if (HaveTraget) 
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (Traget.transform.position - transform.position), 0.2f);
		}
			
	}

	void shot()
	{
		GameObject CurrShell = PhotonNetwork.Instantiate (Shell.name, ShotPoint.position, ShotPoint.rotation, 0, null) as GameObject;
		CurrShell.GetComponent<Shell> ().InitShell (Pv.viewID, Damage);
		CurrShell.GetComponent<Rigidbody> ().AddForce (transform.TransformDirection (new Vector3 (0, Random.Range (-Range, Range) / 100.0f, 1) * Power));
		Pv.RPC ("ShotSound", PhotonTargets.All, null);

	}
	[PunRPC]
	void GetKill()
	{
		Debug.Log ("KillAndSetFalse");
	}

}
