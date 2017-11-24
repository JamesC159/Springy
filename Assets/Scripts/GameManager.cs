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
		float x = Random.Range (minX, maxX);
		float y = Random.Range (minY, maxY);
		float angle = 0f;
		GameObject obj = null;
		Quaternion rotation = Quaternion.identity;
		while (numInstantiated < numColumns) {
			obj = Instantiate (column, new Vector3 (x, y, 0), Quaternion.identity);
//			colliders = Physics2D.OverlapCircle (obj.transform.position, radius);
//			if (colliders != null) {
//				Destroy (obj);
//			} else {
//				numInstantiated++;
//				obj.SetActive (true);
//			}
			numInstantiated++;
			obj.SetActive (true);
			x = Random.Range (minX, maxX);
			y = Random.Range (minY, maxY);
		}
		for (int i = 0; i < numSprings; i++) {
			x = Random.Range (minX, maxX);
			y = Random.Range (4, 7);
			Vector3 location = new Vector3 (x, y, 0);
			Vector3 heading = location - fieldOrigin.transform.position;
			if (heading.x < fieldOrigin.transform.position.x) {
				// Restrict the magnitude of the distance from the field origin by 1/3
			}
			obj = Instantiate (spring, heading, rotation);
			// Find the angle between the field origin's up vector and the proposed spring location
			obj.transform.Rotate(new Vector3(0f, 0, Vector3.SignedAngle(heading, fieldOrigin.transform.up, Vector3.forward)));
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
