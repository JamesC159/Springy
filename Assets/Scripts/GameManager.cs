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

	private int numInstantiated = 0;
	private GameObject[] cols;

	// Use this for initialization
	void Start () {
		float x = Random.Range (3, 17);
		float y = Random.Range (-5, 2);
		for(int i = 0; i < numColumns; i++){
			GameObject obj = Instantiate (column, new Vector3 (x, y, 0), Quaternion.identity);
			colliders = Physics.OverlapSphere (obj.transform.position, radius);
			obj.SetActive (true);
			x = Random.Range (3, 17);
			y = Random.Range (-5, 2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (didWin) {
			// Restart the level with new random columns since the player has beat the level
		}
		if (PlayerController.isDead) {
			// Player died, restart level with new random columns.
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
	}

	void LateUpdate() {
		
	}
}
