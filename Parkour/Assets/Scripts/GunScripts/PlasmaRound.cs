using UnityEngine;
using System.Collections;

public class PlasmaRound : MonoBehaviour {
	float life;
	public GameObject explosion;
	void Update () {
		life += Time.deltaTime;
		if(life>8){
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision){
		Boom ();
	}

	void OnCollisionStay(Collision collision){
		Boom ();
	}

	void Boom(){
		GameObject clone = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
