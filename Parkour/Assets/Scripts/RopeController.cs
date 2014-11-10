using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {
	Rigidbody playerBody;
	Rigidbody anchorBody;
	float maxDistance = 0f;
	float distance = 0f;
	public bool _rope = false;
	public float ropeBounce = 50;
	public bool _dynamic = false;
	PlayerControllerAdvanced player;
	LineRenderer lineRender;
	public GameObject ropeObject;
	CapsuleCollider capsule;
	RopeSegment segment;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControllerAdvanced> ();
		lineRender = gameObject.GetComponent<LineRenderer> ();
	}

	public void CreateRope(Rigidbody anchor, Rigidbody player){
		_rope = true;
		anchorBody = anchor;
		playerBody = player;

		maxDistance = (anchorBody.position - playerBody.position).magnitude;
		lineRender.enabled = true;
		segment = ropeObject.AddComponent<RopeSegment> ();
		capsule = ropeObject.AddComponent<CapsuleCollider> ();
		capsule.radius = 0.05f / 2;
		capsule.center = Vector3.zero;
		capsule.direction = 2;
		capsule.isTrigger = true;
	}

	public void DestroyRope(){
		Destroy (capsule);
		Destroy (segment);
		lineRender.enabled = false;
		_rope = false;
	}
	
	public void RopeAdjust(float dif){
		maxDistance += dif;
	}

	void Update(){
		if(_rope){
			//float dif = Input.GetAxis("Mouse ScrollWheel");
			//if(dif != 0){
			//	RopeAdjust(dif);
			//}

			if(maxDistance<1){maxDistance = 1;}
			distance = (anchorBody.position - playerBody.position).magnitude;
			if(distance>maxDistance){
				if(!_dynamic){
					player.jumps = 1;
				}
				if(_dynamic){
					float mass1 = playerBody.mass;
					float mass2 = anchorBody.mass;
					
					ConstrainTarget(playerBody, anchorBody, 0.1f);
					ConstrainTarget(anchorBody, playerBody, 0.9f);
				} else {
					ConstrainTarget(playerBody, anchorBody, 1f);
				}
			}
			lineRender.SetPosition (0, anchorBody.position);
			lineRender.SetPosition (1, playerBody.position);

			Vector3 half = Vector3.Lerp(anchorBody.position, playerBody.position, 0.5f);
			capsule.transform.position = anchorBody.position + (playerBody.position - anchorBody.position) / 2;
			ropeObject.transform.LookAt(playerBody.position);
			capsule.height = distance - 1;

			//if(Physics.Raycast(playerBody.position, (transform.rotation * -Vector3.right), distance)){
			//}
		}
	}
	

	void ConstrainTarget(Rigidbody from, Rigidbody to, float stepSize){
		float step = distance - maxDistance;
		Vector3 old = from.position;
		from.position = Vector3.MoveTowards(from.position, to.position, step * stepSize) ;
		Vector3 bounce = (old - from.position);
		from.velocity -= bounce * ropeBounce;
	}

	
}
