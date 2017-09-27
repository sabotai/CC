using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic3DPlus_w2 : MonoBehaviour {

	public GameObject mover;
	public int movementAmt = 1;
	public Vector3 startingPosition;
	public GameObject winSpot;

	// Use this for initialization
	void Start () {
		//assign the initial starting position to wherever mover is when the game starts
		startingPosition = mover.transform.position;
	}

	// Update is called once per frame
	void Update () {
		//check if mover's transform.position.* is beyond each threshold
		if (mover.transform.position.z < 0 ||  //is it behind the grid?
			mover.transform.position.z > 6 || //is it too far off the grid?
			mover.transform.position.x < -2 || //is it too far left of the grid?
			mover.transform.position.x > 3) { //is it too far right of the grid?
			mover.transform.position = startingPosition; //if any of those are true... reset it's position to the starting position
		}

		//check if mover's transform.position has the same...
		if (mover.transform.position == 
			new Vector3 (winSpot.transform.position.x, //...x as winSpot
				mover.transform.position.y, //...y as itself, because the winSpot is below it and we don't care about the y
				winSpot.transform.position.z)){ //z as winSpot
			//if so...
			Debug.Log ("win?"); //give us a console message

			mover.GetComponent<MeshRenderer> ().material.color = Color.red; //access the color through 
		}
		if (Input.GetKeyDown("left")) {
			Debug.Log ("left arrow pressed down");
			mover.transform.position += new Vector3(-movementAmt,0,0);
		} 
		if (Input.GetKeyDown("right")) {
			Debug.Log ("right arrow pressed down");
			mover.transform.position += new Vector3(movementAmt,0,0);
		} 
		if (Input.GetKeyDown("up")) {
			Debug.Log ("up arrow pressed down");
			mover.transform.position += new Vector3(0,0,movementAmt);
		} 
		if (Input.GetKeyDown("down")) {
			Debug.Log ("down arrow pressed down");
			mover.transform.position += new Vector3(0,0,-movementAmt);
		} 
	}
}
