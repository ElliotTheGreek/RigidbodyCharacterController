using UnityEngine;
using System.Collections;

public class RopeSegment : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag != "Player"){
		
				//Debug.Log ("Hit Enter on "+other.tag);
		}
	}
	
	void OnCollisionStay(Collision collision){
		collision.collider.isTrigger = true;
		//Debug.Log ("Enter");
			foreach(ContactPoint col in collision.contacts){
				Vector3 point = col.point;
				Debug.Log ("Hit at "+point+ "ON "+collision.gameObject.tag);
			}
	}
}
