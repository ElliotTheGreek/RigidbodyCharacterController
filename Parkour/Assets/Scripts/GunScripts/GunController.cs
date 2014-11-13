using UnityEngine;
using System.Collections;

public class GunController: MonoBehaviour {

	HashIDs hash;
	PlayerControllerAdvanced player;
	Animator anim;

/* the various tool controllers */
	public GrappleController grapple;
	public BlasterController blaster;
	public bool _resetAngle = true;
	public Transform baseTX;
	int totalTools = 2;
	/* 0 = grapple, 1 = blaster */
	int toolInUse = 0;
	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControllerAdvanced> ();
		anim = gameObject.GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HashIDs> ();
		baseTX.rotation = new Quaternion(0,0,0,0);
	}

	void Update(){
		if(_resetAngle){
			//if(baseTX.rotation != null){
			//baseTX.rotation = Quaternion.Lerp(baseTX.rotation, new Quaternion (0,0,0,0), 0.95f);
			baseTX.rotation = new Quaternion (0,0,0,0);
			//}
			
		}
		int currentTool = toolInUse;
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if(scroll>0){toolInUse++;}
		if(scroll<0){toolInUse--;}
	//	Debug.Log (scroll);
		if(toolInUse>totalTools){toolInUse = 0;}
		if(toolInUse<0){toolInUse = totalTools;}
		if(toolInUse == 0){
			grapple.UpdateTool();
			anim.SetBool (hash.grappleOut, true);
		} else {
			if(currentTool == 0){grapple.ResetGrapple();}
			anim.SetBool (hash.grappleOut, false);
		}
		if(toolInUse == 1){
			blaster.UpdateTool();
			anim.SetBool (hash.blasterOut, true);
		} else {
			anim.SetBool (hash.blasterOut, false);
		}
		if(toolInUse == 2){
			anim.SetBool (hash.portalOut, true);
		} else {
			anim.SetBool (hash.portalOut, false);
		}
	}

	public PlayerControllerAdvanced getPlayer(){
		return player;
	}

/* The animation will override the position of rigidbodys so if we want 
 * physics to control something (ie the grapple) we need to disable the animations */
	public void SetAnimation(bool set){
		anim.enabled = set;
	}
}
