using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMove : MonoBehaviour {

	Rigidbody myRb;
	public Rigidbody seeker;
	public GameObject target;
	Vector3 targetPos;
	public bool seek = false;
	public float thrust;


	// Use this for initialization
	void Start () {
		//assign myRb to the rigidbody of the current GameObject
		myRb = GetComponent<Rigidbody>();
	}
	
	//for physics scripts, we use fixedupdate to be less dependent on the framerate
	void FixedUpdate () {
		if (seek){
			//update the targets position each fixedupdate
			targetPos = target.transform.position;
			//by subtracting our objects position from the targets, we get the distance
			//normalizing this means making largest number in the vector3 1 and keeping the ratio with the other axes
			//so if the distance is (4,2,1), meaning 4 units to the right, 2 units above and 1 unit forward
			//it will become (1, 0.5, 0.25), which is a direction instead of a distance
			Vector3 direction = Vector3.Normalize(targetPos - seeker.transform.position);
			//we use the addforce function of Rigidbody to push our object
			//we push it in the direction we calculated (direction of the target from our objects position
			//and multiply it by a force amount
			seeker.AddForce(direction * thrust);
		}

		if (Input.GetKey("up")) {
			myRb.AddForce(transform.forward * thrust);
		}
		if (Input.GetKey ("down")) {
			myRb.AddForce(-transform.forward * thrust);
		}
		if (Input.GetKey ("left")) {
			myRb.AddForce(-transform.right * thrust);
		}
		if (Input.GetKey ("right")) {
			myRb.AddForce(transform.right * thrust);
		}

	}
}