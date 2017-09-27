using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic3D : MonoBehaviour {

	public GameObject mover;
	public Vector3 movementAmount;

	// Use this for initialization
	void Start () {
		movementAmount = new Vector3 (1, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log ("true");
			mover.transform.position += movementAmount;
		} else {
			Debug.Log ("false");
		}
	}
}
