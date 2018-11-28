using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vshot : MonoBehaviour {

	public GameObject Shell;
	public Transform ShotPoint;
	public float ReloadTime;
	public float Power;
	public int Damage;
	public int CurrentShell;
	public int ShellCounts;
	private PhotonView TankPv;
	private PhotonView Pv;
	private bool IsReload = false;
	void Start () {
		TankPv = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	public void Fire()
	{
		
		if (!IsReload) 
		{
			CurrentShell -= 1;
			TankPv.RPC ("ShotSound", PhotonTargets.All, null);
			GameObject CurrShell = PhotonNetwork.Instantiate (Shell.name, ShotPoint.position, ShotPoint.rotation, 0, null) as GameObject;
			CurrShell.GetComponent<Shell> ().InitShell (Pv.viewID, Damage);
			CurrShell.GetComponent<Rigidbody> ().AddForce (CurrShell.transform.forward * Power);
			IsReload = true;
			Invoke ("Reload",ReloadTime);
		}
	}

	void Reload()
	{
		CurrentShell = ShellCounts;
		IsReload = false;
	}

	public void SetPlayerPhotonView(PhotonView pv)
	{
		Pv = pv;
	}
}
