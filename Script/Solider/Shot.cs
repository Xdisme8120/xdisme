using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shot : MonoBehaviour {


	public GameObject Shell;
	public GameObject MedicalBox;
	public GameObject TankerSkillShell;
	public Transform ShotPoint;
	public float ReloadTime;
	public float SkillReadyTime;
	public float ShellDelay;
	public float Range;
	public float Power;
	public int Damage;
	public int ShellCounts;
	public int CurrentShellCount;
	public bool IsReload = false;
	public bool NoAmmo   = false;
	public Text Ammo;
	public AnimatorMW Amw;
	public Image SkillImage;
	public ETCJoystick Etcj;
	public TrailRenderer[] Trail;
	private PhotonView Pv;
	public  bool IsSkillReady = false;
	private float PreSkillImageCold;

	public LineRenderer RpgLine;
	public GameObject HandGun;
	public GameObject RPG;
	public Skill CurrSkill;
	public enum Skill
	{
		Shoter,
		Medical,
		Tanker
	};
	public ShotMode shotMode;
	public enum ShotMode
	{
		Normal,
		TankerSkill
	};
	void Start () {
		SkillInColdTime ();
		PreSkillImageCold = 1/(SkillReadyTime*(1/Time.fixedDeltaTime));
		Amw = GetComponent<AnimatorMW> ();
		Pv = GetComponent<PhotonView> ();
		CurrentShellCount = ShellCounts;
	

	}
	void FixedUpdate()
	{
		if (IsSkillReady == false)
			SkillImage.fillAmount += PreSkillImageCold;
	}
		
	public void Fire()
	{
		if (IsReload)
			return;
		if (NoAmmo) 
		{
			if (!IsReload) 
			{
			//	As.clip = Clips [1];
				//As.Play ();
				Amw.Reload ();
				Invoke ("Reload", ReloadTime);
				IsReload = true;
				return;
			}

		}
		else
		InvokeRepeating ("shot", 0, ShellDelay);

	}
	public void StopFire()
	{
		Amw.StopFire();
		CancelInvoke ("shot");
	}

	void shot()
	{
		if (CurrentShellCount <= 0)
		{	
			NoAmmo = true;
			CancelInvoke ("shot");
			Amw.StopFire();
			return;
		}

		Amw.Fire ();
		CurrentShellCount--;
		switch(shotMode)
		{
		case ShotMode.Normal:
			GameObject CurrShell = PhotonNetwork.Instantiate (Shell.name, ShotPoint.position, ShotPoint.rotation, 0, null) as GameObject;
			CurrShell.GetComponent<Shell> ().InitShell (Pv.viewID, Damage);
			CurrShell.GetComponent<Rigidbody> ().AddForce (transform.TransformDirection (new Vector3 (0, Random.Range (-Range, Range) / 100.0f, 1) * Power));
			Pv.RPC ("ShotSound", PhotonTargets.All, null);
			break;
		case ShotMode.TankerSkill:
			Pv.RPC ("RPGSound", PhotonTargets.All, null);
			GameObject Tankershell = PhotonNetwork.Instantiate (TankerSkillShell.name, ShotPoint.position, ShotPoint.rotation, 0, null) as GameObject;
			Tankershell.GetComponent<Shell> ().InitShell (Pv.viewID, Damage*8);
			Tankershell.GetComponent<Rigidbody> ().AddForce (ShotPoint.transform.TransformDirection (new Vector3 (0,0, 1) * Power*3));
			Amw.animator.SetInteger ("WeaponType_int",1);
			Pv.RPC ("changeweapon", PhotonTargets.All, null);
			shotMode = ShotMode.Normal;
			break;
		}
	}
		
	void Reload()
	{
		CurrentShellCount = ShellCounts;
		IsReload = false;
		NoAmmo = false;
		Amw.ReloadOK();
	}

	public void ButtonReload()
	{
		if (IsReload)
			return;
		Amw.Reload ();
		Invoke ("Reload", ReloadTime);
		IsReload = true;
	}
	public void useSkill()
	{
		Debug.Log ("UseSkill");
		if (IsSkillReady == true) 
		{
			switch (CurrSkill) 
			{
			case Skill.Shoter:
				Etcj.tmSpeed *= 1.5f;
				Damage *= 2;
				Trail [0].gameObject.SetActive (true);
				Trail [1].gameObject.SetActive (true);
				Invoke ("SkillComplete", 8.0f);
				SkillInColdTime ();
				break;
			case Skill.Medical:
				GameObject box = PhotonNetwork.Instantiate (MedicalBox.name, ShotPoint.position, ShotPoint.rotation, 0, null) as GameObject;
				box.GetComponent<Rigidbody> ().AddForce (transform.TransformDirection (new Vector3 (0, 0, 1) * 500));
				SkillInColdTime ();
				break;
			case Skill.Tanker:
				Amw.animator.SetInteger ("WeaponType_int", 8);
				Pv.RPC ("changeweapon", PhotonTargets.All, null);
				shotMode = ShotMode.TankerSkill;
				SkillInColdTime ();
				break;
		
			}
		} else
		{
			return;
		}

	}

	void ReadySkill()
	{
		IsSkillReady = true;
	}

	void SkillInColdTime()
	{
		SkillImage.fillAmount = 0;
		IsSkillReady = false;
		Invoke ("ReadySkill", SkillReadyTime);
	}
	void SkillComplete()
	{
		Etcj.tmSpeed /= 1.5f;
		Damage /= 2;
		Trail [0].gameObject.SetActive (false);
		Trail [1].gameObject.SetActive (false);
	}

	[PunRPC]
	void changeweapon()
	{
		switch(shotMode)
		{
		case ShotMode.Normal:
			RpgLine.enabled = true;
			HandGun.SetActive (false);
			RPG.SetActive (true);
			break;
		case ShotMode.TankerSkill:
			RpgLine.enabled = false;
			HandGun.SetActive (true);
			RPG.SetActive (false);
			break;
		}
	}
}

