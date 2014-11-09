using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour {
	bool _canFire = true;
	ConfigurableJoint joint;
	FixedJoint fixedJoint;
	public Transform gunExit, anchorPlayer;
	GunController gun;
	bool _attached = false;
	public Transform hook;
	RopeController rope;
	void Start(){
		gun = transform.parent.GetComponent<GunController> ();
		rope = gameObject.GetComponent<RopeController> ();
//		lineRender = gameObject.GetComponent<LineRenderer> ();
//		lineRender.enabled = false;
	}

	void Update () {
		if(Input.GetButtonDown("Fire")){
			if(_canFire){
				Fire();
			} else {
				ResetGrapple();
			}
		}
	}

	void ResetGrapple(){
		if(joint != null){
			Destroy (joint);
			joint = null;
		}
		if(fixedJoint != null){
			Destroy(fixedJoint);
			fixedJoint = null;
		}
//		lineRender.SetPosition (1, gunExit.position);

//		lineRender.SetVertexCount (2);
//		lineRender.enabled = false;
		gameObject.GetComponent<RopeController> ().DestroyRope ();

	//	gameObject.GetComponent<RopeScript> ().DestroyRope();
		//Destroy (gameObject.GetComponent<RopeScript> ());
		hook.parent = gun.transform;
		hook.position = new Vector3 (0,0,0);
		hook.rotation = gun.transform.rotation;
		gun.SetAnimation (true);
		_canFire = true;
		_attached = false;
		gun._canFire = true;
		rigidbody.isKinematic = true;
		rigidbody.transform.parent = gun.transform;
	}

	public void ButtonFire(){
		if(_canFire){
			Fire ();
		} else {
			ResetGrapple();
		}
	}

	void Fire(){
	//	lineRender.enabled = true;
		gun.SetAnimation (false);
		_canFire = false;
		gun._canFire = false;
		rigidbody.isKinematic = false;
		rigidbody.transform.parent = null;
		Vector3 force = new Vector3 (0, 0, 1000);
		Transform baseTX = gun.getPlayer().getCamera().gameObject.transform;
		rigidbody.AddForce (baseTX.rotation * force);
	}

	void OnTriggerEnter(Collider other){
		if(!gun._canFire){
			if(other.tag != "Gun" && other.tag != "Player"){
				if(other.rigidbody != null){
					AttachGrapple(other.rigidbody);
				}
			}
		}
	}

	void AttachGrapple(Rigidbody rigid){
		if(!_attached){
			if(rigid.isKinematic){
				Debug.Log ("Attach Kinematic Grapple To "+rigid.tag);
				_attached = true;
				joint = gameObject.AddComponent<ConfigurableJoint>();
				joint.breakTorque = 10000;
				joint.breakForce = 10000;
				joint.xMotion = ConfigurableJointMotion.Locked;
				joint.yMotion = ConfigurableJointMotion.Locked;
				joint.zMotion = ConfigurableJointMotion.Locked;
				joint.angularXMotion = ConfigurableJointMotion.Locked;
				joint.angularYMotion = ConfigurableJointMotion.Locked;
				joint.angularZMotion = ConfigurableJointMotion.Locked;
				joint.enableCollision = true;
				joint.targetPosition = transform.position;
				rope._dynamic = false;
				rope.CreateRope(rigidbody, gun.getPlayer().rigidbody);
			} else {
				//rigidbody.velocity = new Vector3(0,0,0);
				Debug.Log ("Attach Non-Kinematic Grapple To "+rigid.tag);
				_attached = true;
				fixedJoint = gameObject.AddComponent<FixedJoint>();
				fixedJoint.connectedBody = rigid;
				fixedJoint.enableCollision = true;
				rope._dynamic = true;
				rope.CreateRope(rigidbody, gun.getPlayer().rigidbody);
			}
		}
	}

}
