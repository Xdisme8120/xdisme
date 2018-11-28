using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator:MonoBehaviour {

	public Animator MenuAnim;

	void Start()
	{
		MenuAnim = GameObject.Find ("Cam").GetComponent<Animator> ();
	}

	public Animator Animator{
		get{ return MenuAnim;}
	}

	public void StartGame()
	{
		MenuAnim.SetBool ("StartGame", true);
	}

	public void GoRegister()
	{
		MenuAnim.SetBool ("GoRegiester", true);
	}

	public void FinishRegister()
	{
		MenuAnim.SetBool ("GoRegiester", false);
	}

	public void FinishLogin()
	{
		MenuAnim.SetBool ("loginFinish", true);
	}

	public void LoginOut()
	{
		MenuAnim.SetBool ("loginFinish",false);
	}


}
