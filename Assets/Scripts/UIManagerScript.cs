using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

	public GameObject player;
	public static bool restart;
	public static bool quit;
	public Text score;

	private PlayerController playerScript;
	private string scoreText;

	void Awake() {
		playerScript = player.GetComponent<PlayerController> ();
	}

	// Use this for initialization
	void Start () {
		scoreText = "Score: ";
	}

	void Update() {
	}
	
	public void Restart() {
		restart = true;
		playerScript.score = 0;
	}

	public void Quit() {
		quit = true;
		playerScript.score = 0;
	}

	public void UpdateScore() {
		score.text = scoreText + playerScript.score;
	}
}
