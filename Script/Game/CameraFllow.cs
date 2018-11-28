using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraFllow : MonoBehaviour {

	public static CameraFllow _instance;
	public static CameraFllow Instance
	{
		get{ return _instance;}
	}

	public Vector3 smooth;
	public GameObject CurrentTraget;
	public GameObject InitGameObject;

	void awake()
	{
		_instance = this;
	}

	void Start () {
		smooth = transform.position - InitGameObject.transform.position;
		CurrentTraget = InitGameObject;
		Camera.main.aspect=1.78f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Slerp (transform.position, CurrentTraget.transform.position + smooth, 0.06f);
	}
	public void SetCurrentTragert(GameObject Go)
	{
		CurrentTraget = Go;
	}		

}
