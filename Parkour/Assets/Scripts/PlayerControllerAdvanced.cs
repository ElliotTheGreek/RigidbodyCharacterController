using UnityEngine;
using System.Collections;

public class PlayerControllerAdvanced : MonoBehaviour {
	GameController gameController;
	RigidbodyCharacterController rcc;
	public Camera camera;
	public float mouseSensitivity = 2f;
	float verticalAngleLimit = 90.0f;
	float verticalRotation = 0f;
	InputController input;
	public Transform baseAngle;

	public int maxJumps = 3;
	public int jumps = 0;
	void Start () {
		input = GameObject.FindGameObjectWithTag ("GameController").GetComponent<InputController> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		rcc = gameObject.GetComponent<RigidbodyCharacterController> ();
		rcc.maxSpeed = 5f;
	}
	
	void Update () {
		//Transform store = gameObject.AddComponent<Transform> ();
		//store.rotation = transform.rotation;

		baseAngle.LookAt (baseAngle.position + gameController.getGravityVector ());
		baseAngle.Rotate (270, 0, 0);

		Vector3 globalGravityVector = gameController.gravity;
		Vector3 ourDown = transform.rotation.eulerAngles;
		Debug.Log ("T:"+globalGravityVector +" "+ourDown);
//		if(globalGravityVector.x != ourDown.x){
//			transform.rotation = Quaternion.Lerp(transform.rotation, gameController.gravity.rotation, Time.deltaTime * 5);
//		}
//		transform.LookAt ((transform.position + globalGravityVector)) ;

		//transform.up = -gameController.gravity.rotation.eulerAngles;

		/* Rotate our main transform left and right */
		float yaw = 0;
		if(gameController.platform == "Desktop"){ yaw = input.GetAxis ("Mouse X");}
		if(gameController.platform == "Android"){ yaw = input.GetAcceleration().x * 2;}
		transform.Rotate (0, yaw*mouseSensitivity, 0);

		/* Rotate our attached camera up and down
		while staying within our desired angle range */		
		if(gameController.platform == "Desktop"){verticalRotation -= input.GetAxis ("Mouse Y") * mouseSensitivity;}
		if(gameController.platform == "Android"){verticalRotation -= ((input.GetAcceleration().z *2)) * (mouseSensitivity);}


		//	Debug.Log (transform.rotation.eulerAngles);
		//rcc.transform.rotation = Quaternion.Lerp (transform.rotation, store.rotation, Time.deltaTime * 5);

		if(rcc.IsGrounded()){
			float rate = 5f;
			while(verticalRotation>360){ verticalRotation -= 360; }
			while(verticalRotation<-360){ verticalRotation += 360;}
			if(verticalRotation < -verticalAngleLimit){ verticalRotation = Mathf.Lerp(verticalRotation, -verticalAngleLimit, Time.deltaTime * rate);}
			if(verticalRotation > verticalAngleLimit){ verticalRotation = Mathf.Lerp(verticalRotation, verticalAngleLimit, Time.deltaTime * rate);}
			//verticalRotation = Mathf.Clamp (verticalRotation, -verticalAngleLimit, verticalAngleLimit);
		}
		camera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		
		/* Jump Control */
		if(Input.GetButtonDown("Jump")){
			Jump ();
		}
		
		/* Movement control */
		float moveForce = 4f;
		if(!rcc.IsGrounded()){moveForce = 0.05f;}	// reduce player control greatly if in the air ( 0 is most realistic )
		float forwardSpeed = input.GetAxis ("Vertical") * moveForce;
		float sideSpeed = input.GetAxis ("Horizontal") * moveForce;
		
		Vector3 moveVector = new Vector3 (sideSpeed, 0, forwardSpeed);
		moveVector = transform.rotation * moveVector;
		
		rcc.Move (moveVector);

		
	}

	public void Jump(){
		if(rcc.IsGrounded()){
			jumps = 0;
			rcc.Jump(40, 1f);
			jumps++;
		} else {
			if(jumps<maxJumps){
				rcc.Jump(50, 1);
				jumps++;
				float forwardSpeed = input.GetAxis ("Vertical") * 50;
				float sideSpeed = input.GetAxis ("Horizontal") * 50;
				
				Vector3 moveVector = new Vector3 (sideSpeed, 0, forwardSpeed);
				moveVector = transform.rotation * moveVector;
				
				rcc.Nudge (moveVector*5);

			
			}
		}
	}

	public void SetLimitSpeed(bool set){
		rcc._limitSpeed = set;
	}

	public Camera getCamera(){
		return camera;
	}
}
