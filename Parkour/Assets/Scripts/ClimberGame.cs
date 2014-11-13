using UnityEngine;
using System.Collections;

public class ClimberGame : MonoBehaviour {

	public GameObject[] blockTemplates;
	public Transform center;
	PlayerControllerAdvanced player;
	public Rigidbody levelBody;
	float radius = 24f;
	int maxBlocks = 40;
	float maxRange = 4f;
	float minRange = 2f;
	void Start(){
		AddBlocks (0f);
	}

	void AddBlocks(float yLocation){
		for(int i=0;i<maxBlocks;i++){

			int block = Random.Range(0, blockTemplates.Length);

//			if(blockPos.z>wallWidth){blockPos.z = wallWidth - Random.Range(0f, maxRange);}
//			if(blockPos.z<-wallWidth){blockPos.z = -wallWidth + Random.Range(0f, maxRange);}
			yLocation += Random.Range(minRange, maxRange);

			GameObject clone  = RandomOnUnitCircle2(radius, yLocation, block);



			//if(Random.Range(0, 1) == 0){
				clone.rigidbody.constraints = RigidbodyConstraints.None;
				clone.rigidbody.useGravity = true;
				clone.rigidbody.isKinematic = false;
				FixedJoint joint = clone.AddComponent<FixedJoint>();
//				joint.anchor = clone.rigidbody.position;
				//joint.connectedBody = levelBody;
				joint.connectedAnchor = clone.rigidbody.position;
				float breakForce = Random.Range(200f, 500f);
				joint.breakForce = breakForce;
				joint.breakTorque = breakForce;
			//}
		}
	}

	public GameObject RandomOnUnitCircle2( float radius, float yLoc,  int block) 
	{

		GameObject clone = (GameObject) Instantiate (blockTemplates[block], transform.position, transform.rotation);

		Vector2 randomPointOnCircle = Random.insideUnitCircle;

		Vector3 rot = new Vector3 (randomPointOnCircle.x, 0, randomPointOnCircle.y);

		randomPointOnCircle.Normalize();
		randomPointOnCircle *= radius;
		clone.transform.position = new Vector3 (randomPointOnCircle.x, yLoc, randomPointOnCircle.y);
		Vector3 cen = new Vector3 (center.position.x, yLoc, center.position.z);
		clone.transform.LookAt (cen);
		clone.transform.Rotate (new Vector3(90, 90, 0));

		return clone;
	}
}
