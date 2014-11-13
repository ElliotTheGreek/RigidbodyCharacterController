using UnityEngine;
using System.Collections;

public class RigidbodyCharacterController : MonoBehaviour {
/* Public Settings to get desired control effect */
	public bool _FullStop = true;			// When grounded and not attempting to move forward charactor should stop immediately (rather then physics which would gradually slow you)

	bool _moving = false;
	public float verticalVelocity = 0f;
	float distToGround, distToSide;
	float GRAVITY = 100f;
	public bool _leftWall = false, _rightWall = false, _grounded = false;
	public float maxSpeed = 5;
	float stepHeight = 0.35f;
	public bool _limitSpeed = true;
	void Start()
	{
		distToGround = collider.bounds.extents.y;
		distToSide = collider.bounds.extents.x;
		Screen.lockCursor = true;

	}

/* Check for collisions to handle walking up steps */
//	void OnCollisionStay(Collision collision){
//		foreach(ContactPoint col in collision.contacts){
//			Vector3 point = col.point;
//			float difference = point.y - rigidbody.position.y + distToGround - 0.018f;
//			if( (difference >0.01) && (difference<stepHeight)){
//				TryStep(difference, point);
//			}
//		}
//	}

/* Raycast to make sure we are not trying to step up into an object */
	void TryStep(float difference, Vector3 point){
		Debug.Log ("Step up " + difference);
		//if(Physics.Raycast(transform.position, (transform.rotation * Vector3.right), distToSide + 0.1f)){
			Vector3 half = Vector3.Lerp(rigidbody.position, point, 0.5f);
			Vector3 newPos = new Vector3(half.x, point.y + distToGround + 0.2f, half.z);
			rigidbody.position = newPos;
		//}
	}

/* Only apply drag/maxspeed to our forward and side movement (don't drag along gravity) */
	public void LimitSpeed(){
		Vector3 moveSpeed = new Vector3 (rigidbody.velocity.x, 0, rigidbody.velocity.z);
		if(moveSpeed.magnitude > maxSpeed)
		{
			Vector3 store = moveSpeed.normalized * maxSpeed;
			rigidbody.velocity = new Vector3(store.x, rigidbody.velocity.y, store.z);
		}
	}

	/* Jump function takes 2 arguments,
	force is the actual jump force to add
	lerpForce 0 means the falling velocity will not reset, so the upward force will merely be added (more realistic)
 	lerpForce 1 means the falling velocity will be reset to 0 ( generally for for double jumping ) */
	public void Jump(float force, float lerpForce){
		if((rigidbody.velocity.y<0) & (lerpForce>0)){
			float dropSpeed = Mathf.Lerp(rigidbody.velocity.y, 0, lerpForce);
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, dropSpeed, rigidbody.velocity.z);
		}
		verticalVelocity = force;
	}

	void Update () {
	/* Player operating conditions are based on several booleans,  
	 * Update them all in the following function call */
		UpdateBools ();

/* Now manipulate the rigid body's movement   */
		if(_grounded){
			if(_FullStop){
				if(!_moving){rigidbody.drag = 20;} else {rigidbody.drag = 0.5f;}
			}
		} else {
			rigidbody.drag = 0.1f;
			verticalVelocity += (Physics.gravity.y * GRAVITY) * Time.deltaTime;
			//Debug.Log("Vet"+verticalVelocity);
		}



		Vector3 gravity = new Vector3(0, verticalVelocity, 0);
		rigidbody.AddForce (gravity);
		verticalVelocity = 0;
		if(_limitSpeed){
			LimitSpeed ();
		}
	}

	public void Move(Vector3 move){
		rigidbody.AddForce (move, ForceMode.VelocityChange);
	}

	public void Nudge(Vector3 move){
		rigidbody.AddForce (move);
	}

	void UpdateBools(){
		if(Physics.Raycast(transform.position, (transform.rotation * -Vector3.right), distToSide + 0.1f)){
			_leftWall = true;
		} else {
			_leftWall = false;
		}
		
		if(Physics.Raycast(transform.position, (transform.rotation * Vector3.right), distToSide + 0.1f)){
			_rightWall = true;
		} else {
			_rightWall = false;
		}
		
		if(Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f)){
			if(!_grounded){
				if(_FullStop){
					rigidbody.velocity = new Vector3(0, 0, 0);
				}
			}
			_grounded = true;
		} else {
			_grounded = false;
		}
		
		if(( Input.GetAxisRaw("Horizontal") == 0) && ( Input.GetAxisRaw("Vertical") == 0)){
			_moving = false;
		} else {
			_moving = true;
		}
	}

	public bool IsGrounded(){
		return _grounded;
	}
	
	public bool OnLeftWall(){
		return _leftWall;
	}
	
	public bool OnRightWall(){
		return _rightWall;
	}
	

}
