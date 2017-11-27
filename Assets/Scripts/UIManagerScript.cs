﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour {

	public static UIManagerScript instance;
	[HideInInspector]
	public bool restart = false;
	public GameObject UICanvas;
	public bool quit = false;
	public GameObject player;
	public Text score;

	Canvas canvas;
	string scoreText;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else if(instance != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		canvas = UICanvas.GetComponent<Canvas>();
		scoreText = "Score: ";
	}

	void Update() {
		 
	}
	
	public void Restart() {
		// Set restart to true and reset the player's score
		restart = true;
	}

	public void Quit() {
		// Set quit to true and reset the player's score
		quit = true;
	}

	public void UpdateScore() {
		// Update the player's score
		score.text = "Score: " + PlayerController.score;
	}

	public void EnableCanvas() {
		if(canvas != null) {
			canvas.enabled = true;
		}
	}

	public void DisableCanvas() {
		if(canvas != null) {
			canvas.enabled = false;
		}
	}
}
