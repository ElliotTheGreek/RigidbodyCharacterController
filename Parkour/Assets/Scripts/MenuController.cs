using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	
	public void LoadSimple(){
		Application.LoadLevel("Simple");
	}
	
	public void LoadAdvanced(){
		Application.LoadLevel("Advanced");
	}
}
