using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  
  public GameObject column;
	public GameObject ball;
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

  //called before any start function
	void Awake() {
    //check if instance already exists
    if(instance == null)
      //set it to this
      instance = this;
     //if instance already exists and its not this
     else if (instance != this)
      // then destroy this
        Destroy(gameObject);
      //do not destroy when reloading the scene
      DontDestroyOnLoad(gameObject);
      //get a component reference to the sceneManager script 
      sceneScript = GetComponent<SceneManager>();
      //call the start function to initialize the game
      start();
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
		if(ball != null) {
			print("hello");
			PlayerController ballScript = ball.GetComponent<PlayerController>();
			if (ballScript.isDead) {
				// TODO: show player score and ask to quit or continue
				// Player died, restart level with new random columns and reset PlayerController.
				numInstantiated = 0;
				ballScript.isDead = false;
				ballScript.collisionCounter = 0;
				ballScript.hasLaunched = false;
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}
		
	}

	void LateUpdate() {
		
	}
}
