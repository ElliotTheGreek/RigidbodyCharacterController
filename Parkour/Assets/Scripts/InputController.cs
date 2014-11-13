using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class InputController : MonoBehaviour {
	Dictionary<string,float> inputs = new Dictionary<string, float>();
	Vector3 accelOffset = new Vector3(0,0,0);
	public Text debug1, debug2;
	public float GetAxis(string axis){
		if(!inputs.ContainsKey(axis)){
			inputs.Add(axis, 0);
		}
		
		return inputs[axis];
	}
	
	public void SetAxis(string axis, float value){
		Debug.Log ("Set Axis " + axis);
		if(!inputs.ContainsKey(axis)){
			inputs.Add(axis, 0);
		}
		
		inputs[axis] = value;
	}
	
	public void Update()
	{
		if(inputs != null)
		{
			Dictionary<string,float> updates = new Dictionary<string, float>();
			foreach(string key in inputs.Keys)
			{
				float t = Input.GetAxis(key);
				if(t != inputs[key])
				{
					updates.Add (key, t);
				}
			}
			foreach(string key in updates.Keys)
			{
				inputs[key] = updates[key];
			}
		}
		Vector3 newAccel = (Input.acceleration - accelOffset);
	//	debug1.text = "Device Accel \n X:"+Round(Input.acceleration.x) +" Y:"+Round (Input.acceleration.y)+ " Z:"+Round(Input.acceleration.z);
	//	debug2.text = "Accel Offset \n X:"+Round(accelOffset.x) +" Y:"+Round(accelOffset.y)+ " Z:"+Round(accelOffset.z) +"\n New Accel \n X:"+Round(newAccel.x) +" Y:"+Round(newAccel.y)+ " Z:"+Round(newAccel.z);
		//debug2.text = (Input.acceleration - accelOffset) + " From " + accelOffset;
	}

	float Round(float input){
		float rounded = 0;
		rounded = Mathf.Round (input * 100) / 100;
		return rounded;
	}

	public Vector3 GetAcceleration(){
		return (Input.acceleration -  accelOffset);
	}

	public void CalibrateAcceleration(){
		accelOffset = Input.acceleration;
	}
}