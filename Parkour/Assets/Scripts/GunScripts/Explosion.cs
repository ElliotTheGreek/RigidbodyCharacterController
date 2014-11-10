using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	float radius = 5f;
	float power = 25f;

	void Start () {
		Boom ();
	}
	
	void Boom(){
		//Debug.Log ("Boom");
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
		
		foreach (Collider hit in colliders) {
			if (hit && hit.rigidbody)
				hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 3.0f);
		}
	}
}
