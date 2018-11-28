using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VechielBronPoint : MonoBehaviour ,IPunObservable{

	public GameObject BronVechiel;
	public float VechielBronTime;
	private bool hasVechiel = false;

	void Start()
	{
		Bron ();
	}
	public bool HasVechiel
	{
		get{ return hasVechiel; }
		set{ hasVechiel = value; }
	}
		
	[PunRPC]
	public void TankDestory()
	{
		HasVechiel = false;
		ReBronVechiel ();
	}
	public void ReBronVechiel()
	{
		if (HasVechiel)
			return;
		else
		StartCoroutine ("ReBronV");
	}

	IEnumerator ReBronV()
	{
		HasVechiel = true;
		yield return new WaitForSecondsRealtime (VechielBronTime);
		Bron ();
	}

	void Bron()
	{
		if (PhotonNetwork.isMasterClient) {
			GameObject V = PhotonNetwork.InstantiateSceneObject (BronVechiel.name, transform.position, transform.rotation,0,null) as GameObject;
			V.GetComponent<PhotonView> ().RPC ("VechielInit", PhotonTargets.All,gameObject.GetComponent<PhotonView>().viewID);
		}
	}
	public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient) {
			stream.SendNext (hasVechiel);
		} else
			hasVechiel = (bool)stream.ReceiveNext ();
	}
}

