using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private int ShotViewId;
	private int ShellDamage;
	private PhotonView ShellPv;
	private LayerMask AttackMask;
	private LayerMask AttackMask2;
	public float ExplosionRange;
	public AudioClip[] Clips;
	public string[] Tag;
	public GameObject Fx;
	public bool IsTankShell;
	void Start () {
		ShellPv = GetComponent<PhotonView> ();
		AttackMask = LayerMask.GetMask ("Red");
		AttackMask2 = LayerMask.GetMask("Blue");
	}
	


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != this.gameObject) {
			if (other.tag == Tag [0] || other.tag == Tag [1] || other.tag == Tag [2]) {
				other.GetComponent<PhotonView> ().RPC ("GetDamage", PhotonTargets.All, ShellDamage, ShotViewId);
			}

			if (IsTankShell) {
				if (other.tag == Tag [2])
					other.GetComponent<PhotonView> ().RPC ("GetDamage", PhotonTargets.All, ShellDamage * 5, ShotViewId);
				PhotonNetwork.Instantiate (Fx.name, transform.position, transform.rotation, 0);
				AudioSource.PlayClipAtPoint (Clips [0], transform.position);
			}
			if (ShellPv.isMine) {			
				if (IsTankShell) {
					Collider[] Tragets = Physics.OverlapSphere (transform.position, ExplosionRange, AttackMask);
					if (Tragets.Length > 0)
						foreach (Collider C in Tragets) {
							C.gameObject.GetComponent<PhotonView> ().RPC ("GetDamage", PhotonTargets.All, ShellDamage, ShotViewId);
						}
					Collider[] Tragetss = Physics.OverlapSphere (transform.position, ExplosionRange, AttackMask2);
					if (Tragetss.Length > 0)
						foreach (Collider C in Tragetss) {
							C.gameObject.GetComponent<PhotonView> ().RPC ("GetDamage", PhotonTargets.All, ShellDamage, ShotViewId);
						}
				}
				PhotonNetwork.Destroy (this.gameObject);
			}
		}
	}

	public void InitShell(int ViewId,int Damage)
	{
		ShotViewId = ViewId;
		ShellDamage = Damage;
	}
}
