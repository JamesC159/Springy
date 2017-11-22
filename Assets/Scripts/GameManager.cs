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

	[HideInInspector]
	public static bool didWin = false;
	[HideInInspector]
	public static bool restart = false;
	[HideInInspector]
	public static bool quit = false;

	private int numInstantiated = 0;
	private GameObject[] cols;

	// Use this for initialization
	void Start () {
		float x = Random.Range (3, 17);
		float y = Random.Range (-5, 2);
		for(int i = 0; i < numColumns; i++){
			GameObject obj = Instantiate (column, new Vector3 (x, y, 0), Quaternion.identity);
			colliders = Physics.OverlapSphere (obj.transform.position, radius);
			if (colliders.Length > 0) {
				i--;
				Destroy (obj);
			} else {
				obj.SetActive (true);
			}
			x = Random.Range (3, 17);
			y = Random.Range (-5, 2);
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
			// Player died, restart level with new random columns.
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			PlayerController.isDead = false;
		}
	}

	void LateUpdate() {
		
	}
}
