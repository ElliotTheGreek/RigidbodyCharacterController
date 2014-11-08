using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {
	Rigidbody playerBody;
	Rigidbody anchorBody;
	float maxDistance = 0f;
	float distance = 0f;
	bool _rope = false;
	PlayerControllerAdvanced player;
	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControllerAdvanced> ();
		//joint = gameObject.GetComponent<ConfigurableJoint>();
	}

	public void CreateRope(Rigidbody anchor, Rigidbody player){
		_rope = true;
		anchorBody = anchor;
		playerBody = player;

		maxDistance = (anchorBody.position - playerBody.position).magnitude;
	}

	public void DestroyRope(){
		_rope = false;
	}

	void Update(){
		if(_rope){
			float dif = Input.GetAxis("Mouse ScrollWheel");
			maxDistance += dif*2;
			if(maxDistance<1){maxDistance = 1;}
			distance = (anchorBody.position - playerBody.position).magnitude;
			if(distance>maxDistance){
				player.jumps = 1;
				ConstrainTarget();
			}
		}
	}

	void ConstrainTarget(){
		float step = distance - maxDistance;
		Vector3 old = playerBody.position;
		playerBody.position = Vector3.MoveTowards(playerBody.position, anchorBody.position, step) ;
		Vector3 bounce = (old - playerBody.position);
		playerBody.velocity -= bounce * 10;
	//	playerBody.position = Vector3.ClampMagnitude (anchorBody.position, maxDistance);
	}
}
