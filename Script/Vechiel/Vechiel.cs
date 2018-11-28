using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vechiel : MonoBehaviour,IPunObservable {

	private PhotonView Vpv;
	private PhotonView HeadPv;
	private Vshot shot;
	public bool HavePeople = false;
	public PhotonView PlayerPv;

	public int Health;
	public int Defence;
	public float MoveSpeed;
	public GameObject DeadBody;
	public GameObject Head;

	public GameObject VBP;
	private AudioSource Engine;
	private AudioListener Al;
	private Rigidbody Rigidbody;
	private bool IsMove = false;
	void Start () {
		
		Engine = GetComponent<AudioSource> ();
		Al = GetComponent<AudioListener> ();
		Vpv = GetComponent<PhotonView> ();
		HeadPv = Head.gameObject.GetComponent<PhotonView> ();
		shot = GetComponent<Vshot> ();
		Rigidbody = GetComponent<Rigidbody> ();
	}
	
	[PunRPC]
	public void VechielInit(int VBPId)
	{
		VBP = PhotonView.Find (VBPId).gameObject;
	}
	[PunRPC]
	public void VechielAction(int Pvid)
	{
		if (!HavePeople) 
		{
			Engine.Play ();
			PlayerPv = PhotonView.Find (Pvid);
			Vpv.TransferOwnership (PlayerPv.ownerId);
			HeadPv.TransferOwnership (PlayerPv.ownerId);
			HavePeople = true;
			shot.SetPlayerPhotonView (PlayerPv);
			if (PlayerPv.isMine) {
				Al.enabled = true;
			}
		} else 
		{
			if (PlayerPv.isMine) {
				Vpv.TransferOwnership (PhotonNetwork.masterClient.ID);
				HeadPv.TransferOwnership (PhotonNetwork.masterClient.ID);
				Al.enabled = false;
			}
			Engine.Stop ();
			HavePeople = false;
			PlayerPv = null;
		}
	}

	[PunRPC]
	void GetDamage(int _damage,int ViewId)
	{
		Health -= (_damage - Defence);
		if (Health <= 0)
		{
			if (PhotonNetwork.isMasterClient) 
			{
				VBP.GetComponent<PhotonView> ().RPC ("TankDestory", PhotonTargets.All, null);
			}
			PhotonNetwork.Instantiate (DeadBody.name, transform.position, transform.rotation, 0);
			PhotonNetwork.Destroy (this.gameObject);
			if(HavePeople)
			{
				if (PlayerPv.isMine) 
				{
					PlayerPv.gameObject.transform.position = transform.position + new Vector3 (0, 3, 0);
					PlayerPv.RPC ("GetDamage", PhotonTargets.All, 300, ViewId);
					PlayerPv.gameObject.GetComponent<VechielController> ().VechielAction ();
				}
			}
		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient) 
		{
			if(PhotonNetwork.isMasterClient)
				stream.SendNext(HavePeople);
			else
				HavePeople = (bool)stream.ReceiveNext ();
		}
		else 
		{
			if(stream.isWriting)
				stream.SendNext(HavePeople);
			else
				HavePeople = (bool)stream.ReceiveNext ();
		}
	}
}
