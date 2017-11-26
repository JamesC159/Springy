using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

	public static bool restart;
	public static bool quit;
	public GameObject player;
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
		// Set restart to true and reset the player's score
		restart = true;
		playerScript.score = 0;
	}

	public void Quit() {
		// Set quit to true and reset the player's score
		quit = true;
		playerScript.score = 0;
	}

	public void UpdateScore() {
		// Update the player's score
		score.text = scoreText + playerScript.score;
	}
}
