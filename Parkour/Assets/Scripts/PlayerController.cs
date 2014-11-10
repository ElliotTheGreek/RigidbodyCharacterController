using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	RigidbodyCharacterController rcc;
	public Camera camera;
	public float mouseSensitivity = 2f;
	float verticalAngleLimit = 90.0f;
	float verticalRotation = 0f;
	InputController input;
	void Start () {
		input = GameObject.FindGameObjectWithTag ("GameController").GetComponent<InputController> ();
		rcc = gameObject.GetComponent<RigidbodyCharacterController> ();
		rcc.maxSpeed = 5f;
	}
	
	void Update () {
		/* Rotate our main transform left and right */
		float yaw = Input.GetAxis ("Mouse X");
		transform.Rotate (0, yaw*mouseSensitivity, 0);
		
		/* Rotate our attached camera up and down
		while staying within our desired angle range */		
		verticalRotation -= input.GetAxis ("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -verticalAngleLimit, verticalAngleLimit);
		camera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

		/* Jump Control */
		if(Input.GetButtonDown("Jump")){
			if(rcc.IsGrounded()){
				rcc.Jump(500, 0.3f);
			}
		}

		/* Movement control */
		float moveForce = 4f;
		if(!rcc.IsGrounded()){moveForce = 0.1f;}	// reduce player control greatly if in the air ( 0 is most realistic )
		float forwardSpeed = input.GetAxis ("Vertical") * moveForce;
		float sideSpeed = input.GetAxis ("Horizontal") * moveForce;
		
		Vector3 moveVector = new Vector3 (sideSpeed, 0, forwardSpeed);
		moveVector = transform.rotation * moveVector;

		rcc.Move (moveVector);


	}
}
