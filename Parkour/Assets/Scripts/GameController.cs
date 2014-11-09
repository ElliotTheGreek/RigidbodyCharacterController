using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public string platform = "Desktop";
	public GameObject UIDesktop, UIAndroid;
	public Vector3 accelCenter = new Vector3(0,0,0);

	void Start () {
		if (Application.platform == RuntimePlatform.Android){
			platform = "Android";
			UIAndroid.SetActive(true);
			UIDesktop.SetActive(false);
		} else {
			platform = "Desktop";
			UIAndroid.SetActive(false);
			UIDesktop.SetActive(true);
		}
	}

	public void Calibrate(){
		accelCenter = Input.acceleration;
	}
}
