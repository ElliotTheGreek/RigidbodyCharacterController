using UnityEngine;
using System.Collections;

public class PlayerControllerAdvanced : MonoBehaviour {
	GameController gameController;
	RigidbodyCharactorController rcc;
	public Camera camera;
	public float mouseSensitivity = 2f;
	float verticalAngleLimit = 90.0f;
	float verticalRotation = 0f;

	public int maxJumps = 3;
	public int jumps = 0;
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		rcc = gameObject.GetComponent<RigidbodyCharactorController> ();
		rcc.maxSpeed = 5f;
	}
	
	void Update () {
		/* Rotate our main transform left and right */
		float yaw = 0;
		if(gameController.platform == "Desktop"){ yaw = Input.GetAxis ("Mouse X");}
		if(gameController.platform == "Android"){ yaw = Input.acceleration.x;}
		transform.Rotate (0, yaw*mouseSensitivity, 0);
		Debug.Log ("Mode:"+gameController.platform);
		/* Rotate our attached camera up and down
		while staying within our desired angle range */		
		if(gameController.platform == "Desktop"){verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;}
		if(gameController.platform == "Android"){verticalRotation -= ((Input.acceleration.y - gameController.accelCenter.y) * -1) * mouseSensitivity;}

		verticalRotation = Mathf.Clamp (verticalRotation, -verticalAngleLimit, verticalAngleLimit);
		camera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		
		/* Jump Control */
		if(Input.GetButtonDown("Jump")){
			Jump ();
		}
		
		/* Movement control */
		float moveForce = 4f;
		if(!rcc.IsGrounded()){moveForce = 0.1f;}	// reduce player control greatly if in the air ( 0 is most realistic )
		float forwardSpeed = Input.GetAxis ("Vertical") * moveForce;
		float sideSpeed = Input.GetAxis ("Horizontal") * moveForce;
		
		Vector3 moveVector = new Vector3 (sideSpeed, 0, forwardSpeed);
		moveVector = transform.rotation * moveVector;
		
		rcc.Move (moveVector);
		
		
	}

	public void Jump(){
		if(rcc.IsGrounded()){
			jumps = 0;
			rcc.Jump(500, 1f);
			jumps++;
		} else {
			if(jumps<maxJumps){
				rcc.Jump(500, 0.9f);
				jumps++;
			}
		}
	}
	
	public Camera getCamera(){
		return camera;
	}
}
