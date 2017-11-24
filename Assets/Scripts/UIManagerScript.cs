using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour {

	public static bool restart;
	public static bool quit;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
	}
	
	public void Restart() {
		restart = true;
	}

	public void Quit() {
		quit = true;
	}
}
