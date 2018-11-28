using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMW : MonoBehaviour {

	public Animator animator;
	public SoliderInfo SINFO;

	void Start () {
		SINFO = GetComponent<SoliderInfo> ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetFloat ("Speed_f",Mathf.Abs(SINFO.Etcj.axisX.axisValue*10)+Mathf.Abs(SINFO.Etcj.axisY.axisValue*10));

	}

	public void Fire()
	{
		animator.SetBool ("Shoot_b", true);	
	}
	public void StopFire()
	{
		animator.SetBool ("Shoot_b", false);	
	}

	public void Reload()
	{
		animator.SetBool ("Reload_b", true);	
	}
	public void ReloadOK()
	{
		animator.SetBool ("Reload_b", false);	
	}
	public void Die()
	{
		SINFO.GetV.gameObject.SetActive (false);
		SINFO.Etcj.gameObject.SetActive (false);
		SINFO.Fire.gameObject.SetActive (false);
		animator.SetBool ("Death_b", true);
	}
}
