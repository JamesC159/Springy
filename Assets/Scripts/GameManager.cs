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
		for(int i = 0; i < numColumns; i++) {
			location = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
			obj = Instantiate (column, location, rotation);
			obj.SetActive(true);
		}
		for (int i = 0; i < numSprings; i++) {
			location = new Vector3 (Random.Range (minX, maxX), Random.Range (4, 7), 0);
			Vector3 heading = location - fieldOrigin.transform.position;
			if (heading.x < fieldOrigin.transform.position.x) {
				// Restrict the magnitude of the distance from the field origin by 1/3
			}
			obj = Instantiate (spring, location, rotation);
			// Find the angle between the field origin's up vector and the proposed spring location
			print(Vector3.SignedAngle(fieldOrigin.transform.up, heading, Vector3.forward));
			if((int)Random.Range(0, 1000000000) % 2 == 0) {
				if((int)Random.Range(0, 1000000000) % 2 == 0) {
					delta = angleDelta * -1f;
				}
				obj.transform.Rotate(new Vector3(0f, 0, Vector3.SignedAngle(fieldOrigin.transform.up, heading, Vector3.forward) + delta));

			} else {
				obj.transform.Rotate(new Vector3(0f, 0, Vector3.SignedAngle(fieldOrigin.transform.up, heading, Vector3.forward) + delta));
			}
			obj.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (didWin) {
//			// Display the player's score and ask to rety or quit
//			if (restart) {
//				// If restart, reload the scene
//			} else if (quit) {
//				// If quit, take user back to main menu
//			}
//		}
		if (UIManagerScript.restart) {
			canvas.enabled = !canvas.enabled;
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			UIManagerScript.restart = false;
		} else if (UIManagerScript.quit) {
			UIManagerScript.quit = false;
			print("Quitting");
		}
		if(ball != null) {
			PlayerController ballScript = ball.GetComponent<PlayerController>();
			if (ballScript.isDead) {
				// Enable to UI Canvas
				if(uiCanvas != null && UIManager != null) {
					UIManagerScript ms = UIManager.GetComponent<UIManagerScript> ();
					ms.UpdateScore ();
					canvas = uiCanvas.GetComponent<Canvas>();
					canvas.enabled = !canvas.enabled;
				}
				// Player died, restart level with new random columns and reset PlayerController.
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
