using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public string platform = "Desktop";
	public GameObject UIDesktop, UIAndroid;

	void Start () {
		if (Application.platform == RuntimePlatform.Android){
			platform = "Android";
			EnableUI("UI-Android", true);
			EnableUI("UI-Desktop", false);
		} else {
			platform = "Desktop";
			EnableUI("UI-Desktop", true);
			EnableUI("UI-Android", false);
		}
	}

	void EnableUI(string tag, bool set){
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject obj in objects){
			obj.SetActive(set);
		}
	}
}
