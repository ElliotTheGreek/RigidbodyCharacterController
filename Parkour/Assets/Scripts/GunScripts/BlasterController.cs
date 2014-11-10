using UnityEngine;
using System.Collections;

public class BlasterController : MonoBehaviour {
	public GameObject plasmaRound;
	public Transform gunExit;
	GunController gun;

	void Start(){
		gun = transform.parent.GetComponent<GunController> ();
	}

	public void UpdateTool () {
		if(Input.GetButtonDown("Fire")){
			Fire();
		}
	}

	void Fire(){
		GameObject clone = (GameObject) Instantiate (plasmaRound, gunExit.position, gunExit.rotation);
		Vector3 force = new Vector3 (0, 0, 500);
		Transform baseTX = gun.getPlayer().getCamera().gameObject.transform;
		clone.rigidbody.velocity = gun.transform.parent.transform.parent.rigidbody.velocity;
		clone.rigidbody.AddForce (baseTX.rotation * force);
	}
}
