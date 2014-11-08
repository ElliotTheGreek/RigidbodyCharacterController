using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour {
	bool _canFire = true;
	ConfigurableJoint joint;
	LineRenderer lineRender;
	public Transform gunExit;
	GunController gun;
	bool _attached = false;
	public Transform hook;
	void Start(){
		gun = transform.parent.GetComponent<GunController> ();
		lineRender = gameObject.GetComponent<LineRenderer> ();
		lineRender.enabled = false;
	}

	void Update () {
		if(Input.GetButtonDown("Fire")){
			if(_canFire){
				Fire();
			} else {
				ResetGrapple();
			}
		}
		lineRender.SetPosition (0, transform.position);
		lineRender.SetPosition (1, gunExit.position);
	}

	void ResetGrapple(){
		Destroy (joint);

		lineRender.SetVertexCount (2);
		lineRender.enabled = false;
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

	void Fire(){
		lineRender.enabled = true;
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
			gameObject.GetComponent<RopeController>().CreateRope(rigidbody, gun.getPlayer().rigidbody);
			//gameObject.GetComponent<RopeScript> ().enabled = true;
			//gameObject.GetComponent<RopeScript> ().target = gunExit;

			//gameObject.GetComponent<RopeScript> ().BuildRope();
		}
	}

}
