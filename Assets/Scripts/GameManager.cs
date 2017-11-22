using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	public GameObject ball;
	public Collider[] colliders;
	public GameObject column;
	public int numColumns;
	public float radius = 0f;
	public float minX;
	public float minY;
	public float maxX;
	public float maxY;

	[HideInInspector]
	public static bool didWin = false;
	[HideInInspector]
	public static bool restart = false;
	[HideInInspector]
	public static bool quit = false;
	[HideInInspector]
	public static int numInstantiated = 0;


	private GameObject[] cols;

	// Use this for initialization
	void Start () {
		float x = Random.Range (minX, maxX);
		float y = Random.Range (minY, maxY);
		while(numInstantiated < numColumns) {
			GameObject obj = Instantiate (column, new Vector3 (x, y, 0), Quaternion.identity);
			obj.SetActive (true);
			x = Random.Range (minX, maxX);
			y = Random.Range (minY, maxY);
			numInstantiated++;
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
		if (PlayerController.isDead) {
			// TODO: show player score and ask to quit or continue
			// Player died, restart level with new random columns.
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			numInstantiated = 0;
			PlayerController.isDead = false;
		}
	}

	void LateUpdate() {
		
	}
}
