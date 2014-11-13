using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour {
	bool _canFire = true;
	ConfigurableJoint joint;
	FixedJoint fixedJoint;
	public Transform gunExit, anchorPlayer, gunBase;
	GunController gun;
	bool _attached = false;
	public Transform hook;
	RopeController rope;
	public Rigidbody baseBody;
	void Start(){
		gun = transform.parent.GetComponent<GunController> ();
		rope = gameObject.GetComponent<RopeController> ();
	}

	public void UpdateTool () {
		if(Input.GetButtonDown("Fire")){
			if(_canFire){
				Fire();
			} else {
				ResetGrapple();
			}
		}
		if(!_canFire){
			if(Input.GetButton("ToolUp")){
				RopeIn();
			}
			if(Input.GetButton("ToolDown")){
				RopeOut();
			}
		}
	}

	public void ResetGrapple(){
		gun._resetAngle = true;

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
		rigidbody.isKinematic = true;
		rigidbody.transform.parent = gun.transform;
		gameObject.GetComponent<SphereCollider> ().enabled = true;

	}
	
	public void RopeOut(){
		rope.RopeAdjust (0.25f);
	}
	
	public void RopeIn(){
		rope.RopeAdjust (-0.1f);
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
		rigidbody.isKinematic = false;
		rigidbody.transform.parent = null;
		Vector3 force = new Vector3 (0, 0, 1500);
		Transform baseTX = gun.getPlayer().getCamera().gameObject.transform;
		rigidbody.velocity = Vector3.Lerp(baseBody.velocity, new Vector3(0,0,0), 0);
		rigidbody.AddForce (baseTX.rotation * force);
		gameObject.GetComponent<SphereCollider> ().enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if(!_canFire){
			if(other.tag != "Gun" && other.tag != "Player"){
				if(other.rigidbody != null){
					AttachGrapple(other.rigidbody);
				}
			}
		}
	}

	void OnGUI()
	{
		if(!_canFire && rope._rope)
		{
			if (GUI.RepeatButton(new Rect((Screen.width) - 160, (Screen.height/2) + 10, 150, 100), new GUIContent("Release Rope [g]"))) {
				RopeOut();
			}
			if (GUI.RepeatButton(new Rect((Screen.width) - 160, (Screen.height/2) - 90, 150, 100), new GUIContent("Retract Rope [t]"))) {
				RopeIn();
			}
		}
	}

	void AttachGrapple(Rigidbody rigid){
		if(!_attached){
			gun._resetAngle = false;
			if(rigid.isKinematic){
				//Debug.Log ("Attach Kinematic Grapple To "+rigid.tag);
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
				rope.CreateRope(rigidbody, gun.getPlayer().rigidbody, gunBase.transform);
			} else {
				//rigidbody.velocity = new Vector3(0,0,0);
				//Debug.Log ("Attach Non-Kinematic Grapple To "+rigid.tag);
				_attached = true;
				fixedJoint = gameObject.AddComponent<FixedJoint>();
				fixedJoint.connectedBody = rigid;
				fixedJoint.enableCollision = true;
				rope._dynamic = true;
				rope.CreateRope(rigidbody, gun.getPlayer().rigidbody, gunBase.transform);
			}
		}
	}

}
