using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MachineGunBox :BuildingInfo,BuildingInterface {

	public GameObject MachineGun;
	public Slider slider;

	public override void BuildingDamageEvent()
	{
		PhotonNetwork.Destroy(slider.gameObject);
		PhotonNetwork.Destroy (MachineGun);
	}

}
