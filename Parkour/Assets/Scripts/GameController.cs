using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public string platform = "Desktop";
	public GameObject UIDesktop, UIAndroid;

	public Vector3 gravity = new Vector3(0, -10, 0);

	void Start () {
		if (Application.platform == RuntimePlatform.Android){
			platform = "Android";
			EnableUI("UI-Android", true);
			EnableUI("UI-Desktop", false);
		} else if (Application.platform == RuntimePlatform.IPhonePlayer){
			platform = "Android";
			EnableUI("UI-Desktop", false);
			EnableUI("UI-Android", true);
		} else {
			platform = "Desktop";
			EnableUI("UI-Desktop", true);
			EnableUI("UI-Android", false);
		}
	}
	
	void Update(){
	}

	void EnableUI(string tag, bool set){
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject obj in objects){
			obj.SetActive(set);
		}
	}

	public Vector3 getGravityVector(){
		return gravity;
	}
}
