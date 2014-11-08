using UnityEngine;
using System.Collections;

public class PlayerControllerAdvanced : MonoBehaviour {
	RigidbodyCharactorController rcc;
	public Camera camera;
	public float mouseSensitivity = 2f;
	float verticalAngleLimit = 90.0f;
	float verticalRotation = 0f;

	public int maxJumps = 2;
	public int jumps = 0;
	void Start () {
		rcc = gameObject.GetComponent<RigidbodyCharactorController> ();
		rcc.maxSpeed = 5f;
	}
	
	void Update () {
		/* Rotate our main transform left and right */
		float yaw = Input.GetAxis ("Mouse X");
		transform.Rotate (0, yaw*mouseSensitivity, 0);
		
		/* Rotate our attached camera up and down
		while staying within our desired angle range */		
		verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -verticalAngleLimit, verticalAngleLimit);
		camera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		
		/* Jump Control */
		if(Input.GetButtonDown("Jump")){
			if(rcc.IsGrounded()){
				jumps = 0;
				rcc.Jump(500, 1f);
				jumps++;
			} else {
				if(jumps<maxJumps){
					rcc.Jump(500, 0.5f);
					jumps++;
				}
			}
		}
		
		/* Movement control */
		float moveForce = 4f;
		if(!rcc.IsGrounded()){moveForce = 0.2f;}	// reduce player control greatly if in the air ( 0 is most realistic )
		float forwardSpeed = Input.GetAxis ("Vertical") * moveForce;
		float sideSpeed = Input.GetAxis ("Horizontal") * moveForce;
		
		Vector3 moveVector = new Vector3 (sideSpeed, 0, forwardSpeed);
		moveVector = transform.rotation * moveVector;
		
		rcc.Move (moveVector);
		
		
	}

	public Camera getCamera(){
		return camera;
	}
}
