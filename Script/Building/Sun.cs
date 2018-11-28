using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour{

	public float RotateSpeed;

	public Light[] lights;
	private Light Dir;
	public float Time=0;
	void Start () {
		Dir = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Time = transform.localEulerAngles.x;
		transform.Rotate (new Vector3 (RotateSpeed, 0, 0));

		if (Time >15&&Time<=90)
			Morning ();
		else if (Time >250 ||Time <15)
			Night();
		
	}

	void Morning()
	{
		
		RotateSpeed =0.01f;
		Dir.intensity = Mathf.Lerp (Dir.intensity, 1.0f, 0.005f);
		foreach (Light L in lights)
			L.enabled = false;
	}
	void Night()
	{
		RotateSpeed = 0.06f;
		Dir.intensity = Mathf.Lerp (Dir.intensity, 0, 0.005f);
		foreach (Light L in lights)
			L.enabled = true;
	}
}
