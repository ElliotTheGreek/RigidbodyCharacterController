using UnityEngine;
using System.Collections;

public class DynamicObject : MonoBehaviour {

	GameController game;
	ConstantForce gravity;

	void Awake(){
		game = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gravity = gameObject.AddComponent<ConstantForce> ();
	}

	void Update () {
		gravity.force = game.getGravityVector();
	}
}
