using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingInfo : MonoBehaviour,IPunObservable {

	public int Health;   
	public Slider HealthSlider;
	public GameObject DeadBody;
	public PhotonView Pv;
	public AudioSource As;
	public Canvas DamageInfo;
	public enum Team
	{
		Blue,
		Red
	};
	public Team team;
	// Update is called once per frame
	void Update () {
		
	}
		
	public virtual void BuildingDamageEvent()
	{
		switch (team) {
		case Team.Blue:
			UiManager.Instance.GameOver (0);
			break;
		case Team.Red:
			UiManager.Instance.GameOver (1);
			break;
		}

	}
	[PunRPC]
	public void GetDamage(int Damage,int ViewID)
	{
		Health -= Damage;
		As.Play ();
		HealthSlider.value = Health;
		if (PhotonNetwork.isMasterClient) {
			GameObject damageInfo = PhotonNetwork.Instantiate (DamageInfo.name, transform.localPosition, transform.localRotation, 0) as GameObject;
			damageInfo.GetComponent<Text> ().text = "" + Damage + "";
			if (Health <= 0) {
				BuildingDamageEvent ();
				PhotonNetwork.Instantiate (DeadBody.name, transform.position, transform.rotation, 0);
				PhotonNetwork.Destroy (HealthSlider.gameObject);
				PhotonNetwork.Destroy (this.gameObject);

			}
		}

		
	}

	public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient) {
			stream.SendNext (Health);
		} else
			Health = (int)stream.ReceiveNext ();
	}
}
