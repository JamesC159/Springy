using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject column;
	public GameObject spring;
	public GameObject ball;
	public GameObject uiCanvas;
	public GameObject UIManager;
	public GameObject fieldOrigin;
	public int numColumns;
	public int numSprings;
	public float radius = 0f;
	public float minX;
	public float minY;
	public float maxX;
	public float maxY;
	public float minSpringSpawnRotation;
	public float maxSpringSpawnRotation;
	public float angleDelta;

	[HideInInspector]
	public bool didWin = false;
	[HideInInspector]
	public bool restart = false;
	[HideInInspector]
	public bool quit = false;
	[HideInInspector]
	public int numInstantiated = 0;

	private GameObject[] cols;
	private Canvas canvas;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		float angle = 0f;
		float delta = 0f;
		GameObject obj = null;
		Vector3 location = Vector3.zero;
		Quaternion rotation = Quaternion.identity;

		// Randomly Instantiate columns and springs
		for(int i = 0; i < numColumns; i++) {
			// Choose a random location for the column and spring
			location = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
			obj = Instantiate (column, location, rotation);
			// Activate the coumn and spring
			obj.SetActive(true);
		}
		for (int i = 0; i < numSprings; i++) {
			// Randomly choose a location for the spring
			location = new Vector3 (Random.Range (minX, maxX), Random.Range (4, 7), 0);
			// Calculate the vector from field origin to location for rotation quaternion
			Vector3 heading = location - fieldOrigin.transform.position;
			if (heading.x < fieldOrigin.transform.position.x) {
				// Restrict the magnitude of the distance from the field origin by 1/3
			}
			obj = Instantiate (spring, location, rotation);
			// Randomly choose if we are offsetting the rotation of the spring by angleDelta
			if((int)Random.Range(0, 10000000000) % 3 == 0) {
				// Randomly choose if we are negating the delta
				if((int)Random.Range(0, 10000000000) % 2 == 0) {
					delta = angleDelta * -1f;
				}
				obj.transform.Rotate(new Vector3(0f, 0, Vector3.SignedAngle(fieldOrigin.transform.up, heading, Vector3.forward) + delta));

			} else {
				obj.transform.Rotate(new Vector3(0f, 0, Vector3.SignedAngle(fieldOrigin.transform.up, heading, Vector3.forward)));
			}
			// Activate the spring
			obj.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Handle UI Interactions
		if (UIManagerScript.restart) {
			// Disable the canvas and reload the scene
			canvas.enabled = !canvas.enabled;
			UIManagerScript.restart = false;
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		} else if (UIManagerScript.quit) {
			// Quit the game
			Application.Quit();
		}
		if(ball != null) {
			PlayerController ballScript = ball.GetComponent<PlayerController>();
			// If the user has died
			if (ballScript.isDead) {
				if(uiCanvas != null && UIManager != null) {
					// Update the score UI text
					UIManagerScript ms = UIManager.GetComponent<UIManagerScript> ();
					ms.UpdateScore ();
					// Enable the UI Canvas
					canvas = uiCanvas.GetComponent<Canvas>();
					canvas.enabled = !canvas.enabled;
				}
				// Player died, reset PlayerController.
				numInstantiated = 0;
				ballScript.isDead = false;
				ballScript.collisionCounter = 0;
				ballScript.hasLaunched = false;
			}
		}
		
	}

	void LateUpdate() {
		
	}
}
