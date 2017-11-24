using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject column;
	public GameObject ball;
	public GameObject uiCanvas;
	public int numColumns;
	public float radius = 0f;
	public float minX;
	public float minY;
	public float maxX;
	public float maxY;

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
		while(numInstantiated < numColumns) {
			GameObject obj = Instantiate (column, new Vector3 (x, y, 0), Quaternion.identity);
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
	}
	
	// Update is called once per frame
	void Update () {
		if (didWin) {
			// Display the player's score and ask to rety or quit
			if (restart) {
				// If restart, reload the scene
			} else if (quit) {
				// If quit, take user back to main menu
			}
		}
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
				// TODO: show player score and ask to quit or continue
				// Player died, restart level with new random columns and reset PlayerController.
				numInstantiated = 0;
				ballScript.isDead = false;
				ballScript.collisionCounter = 0;
				ballScript.hasLaunched = false;
				// Enable to UI Canvas
				if(uiCanvas != null) {
					canvas = uiCanvas.GetComponent<Canvas>();
					canvas.enabled = !canvas.enabled;
				}
			}
		}
		
	}

	void LateUpdate() {
		
	}
}
