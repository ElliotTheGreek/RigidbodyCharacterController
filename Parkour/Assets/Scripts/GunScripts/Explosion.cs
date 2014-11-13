using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	float radius = 16f;
	float power = 190f;
	public Light light;
	float life = 0;
	float targetIntensity = 0;
	void Start () {
		Boom ();
	}

	void Update(){
		life += Time.deltaTime;
		light.intensity -= Time.deltaTime * 2;
		if(light.intensity<0){light.intensity = 0;}
	}
	
	void Boom(){
		targetIntensity = 1f;
		//Debug.Log ("Boom");
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
		
		foreach (Collider hit in colliders) {
			if (hit && hit.rigidbody)
				hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 3.0f);
		}
	}
}
