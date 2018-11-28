using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThing : MonoBehaviour {


	public float RotetaSpeed;

	void FixedUpdate () {

			transform.Rotate (new Vector3 (0, RotetaSpeed, 0));

	}
}
