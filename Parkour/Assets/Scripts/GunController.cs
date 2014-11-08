using UnityEngine;
using System.Collections;

public class GunController: MonoBehaviour {

	public bool _canFire = true;

	PlayerControllerAdvanced player;
	Animator anim;
	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControllerAdvanced> ();
		anim = gameObject.GetComponent<Animator> ();
	}

	public PlayerControllerAdvanced getPlayer(){
		return player;
	}

	public void SetAnimation(bool set){
		anim.enabled = set;
	}
}
