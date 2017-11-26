using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// GameObjects
	public GameObject column;
	public GameObject spring;
	public GameObject ball;
	public GameObject launchSpring;
	public GameObject skyTile;
	public GameObject grassTile;
	public GameObject UICanvas;
	public GameObject UIManager;
	public GameObject fieldOrigin;

	// Object pools
	public int columnPoolSize;
	public int springPoolSize;
	public int skyPoolSize;
	public int grassPoolSize;

	// Custom GameObject instantiation variables
	public float radius = 0f;
	public float minColSeparation;
	public float minSpringSeparation;
	public float minColX;
	public float minSpringX;
	public float minColY;
	public float minSpringY;
	public float maxColX;
	public float maxSpringX;
	public float maxColY;
	public float maxSpringY;
	public float angleDelta;

	// Object pools
	List<GameObject> columnPool;
	List<GameObject> springPool;
	List<GameObject> skyPool;
	List<GameObject> grassPool;

	void Awake() {
		// Initialize object pools
		columnPool = new List<GameObject>();
		springPool = new List<GameObject>();
		skyPool = new List<GameObject>();
		grassPool = new List<GameObject>();
		InitObjectPool(columnPool, columnPoolSize);
		InitObjectPool(springPool, springPoolSize);
		InitObjectPool(skyPool, skyPoolSize);
		InitObjectPool(grassPool, grassPoolSize);
	}


	// Use this for initialization
	void Start () {
		// Instantiate the background
		InstantiateBackground(skyTile, skyPool, skyPoolSize);
		InstantiateBackground(grassTile, grassPool, grassPoolSize);
		// Randomly Instantiate columns and springs
		InstantiateColumns();
		// Randomly Instantiate springs in the sky
		InstantiateSprings();
		// Activate the launch spring and ball 
		UpdateObjectActiveStatus(launchSpring);
		UpdateObjectActiveStatus(ball);
	}
	
	// Update is called once per frame
	void Update () {

		// Handle UI Interactions
		if (UIManagerScript.restart) {
			StartCoroutine(Restart());// Disable the canvas and reload the scene
		} else if (UIManagerScript.quit) {
			// Quit the game
			Application.Quit();
		}

		// Handle live game state
		if(ball != null) {
			PlayerController ballScript = ball.GetComponent<PlayerController>();
			// If the user has died
			if (ballScript.isDead) {
				if(UICanvas != null && UIManager != null) {
					// Update the player's score
					UpdatePlayerScore(UIManager.GetComponent<UIManagerScript>());
					// Enable the UI Canvas
					UpdateCanvas(UICanvas.GetComponent<Canvas>());
				}
				// Player died, reset PlayerController.
				ballScript.isDead = false;
				ballScript.collisionCounter = 0;
				ballScript.hasLaunched = false;
			}
		}

		// Cycle grass and sky positions to keep seemless background
		if(CameraController.moveGrassAndSky) {
			// CycleObjectPositions(grassPool);
			CycleObjectPositions(skyPool);
			CycleObjectPositions(grassPool);
			CameraController.moveGrassAndSky = false;
		}
	}

	void InitObjectPool(List<GameObject> pool, int poolSize) {
		GameObject dummy = null;
		for(int i = 0; i < poolSize; i++) {
			pool.Add(dummy);
		}
	}

	void CycleObjectPositions(List<GameObject> pool) {
		GameObject firstObj = pool[0];
		GameObject lastObj = pool[pool.Count - 1];
		Vector2 size = firstObj.GetComponent<BoxCollider2D>().size;
		size.x /= 2f;
		size.y = 0f;
		Vector3 size3 = new Vector3(size.x, size.y, 0f);
		firstObj.transform.position = lastObj.transform.position + size3;
		pool.Add(firstObj);
		pool.RemoveAt(0);
	}

	bool CoinFlip() {
		// Simulate randomness
		int counter = 0;
		for(int i = 1; i <= 15; i++) {
			if((int)Random.Range(0, 10000000000) % i == 0) {
				counter++;
			}
		}
		// Make sure we aren't simply dividing 0
		if(counter != 0) {
			return (counter % 3 == 0) ? true : false;
		} else {
			return (Random.Range(0, 10000000000) % 9 == 0) ? true : false;
		}
	}

	bool CheckOverlap(GameObject obj, List<GameObject> objects) {
		// For each object being compared to the target object:
		if(obj != null && objects != null) {
			foreach(GameObject o in objects) {
				if(o != null) {
					// Get the distance between the objects
					float d = Vector3.Distance(o.transform.position, obj.transform.position);
					// Check if column or spring is overlapping with any objects
					if(obj.tag == "Column" 
						&& d < minColSeparation) {
						return true;
					} else if (obj.tag == "Spring" 
						&& d < minSpringSeparation) {
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	IEnumerator RotateSkySpring(GameObject skySpring, 
						GameObject worldOrigin, 
						Vector3 position, 
						float rotOffset) {
		// Rotate the sky spring by the angle between the origin of the world
		// and the position of the sky spring, with a chance of an offset angle
		// added to the rotation
		if(skySpring && worldOrigin) {
			skySpring.transform.Rotate(
				new Vector3(0f, 
							0f, 
							Vector3.SignedAngle(
									worldOrigin.transform.up, 
									position, 
									Vector3.forward) 
							+ rotOffset));
		}

		yield return null;
	}

	void UpdateObjectActiveStatus(GameObject obj) {
		if(obj != null) {
			obj.SetActive(!obj.activeSelf);
			// Activate children a column
			if(obj.tag == "Column") {
				foreach(Transform child 
					in obj.transform.GetComponentsInChildren<Transform>()) {
					child.gameObject.SetActive(true);
				}
			}
		}
	}

	void InstantiateColumns() {
		GameObject obj = null;
		Vector3 location = Vector3.zero;
		Vector3 heading = Vector3.zero;
		Quaternion rotation = Quaternion.identity;

		if(column != null){
			for(int i = 0; i < columnPoolSize; i++) {
				// Choose a random location for the column and spring
				location = new Vector3(Random.Range(minColX, maxColX), 
									Random.Range(minColY, maxColY), 
									0f);
				// Insantiate the new GameObject
					obj = Instantiate (column, 
								location, 
								rotation);
				// If this object overlaps with any other objects, destroy the new object
				if(CheckOverlap(obj, columnPool)) {
					Destroy(obj);
					// Decrement i to try again
					i--;
				} else {
					// Add the column to the column pool
					columnPool[i] = obj;
					// Activate the coumn and spring
					UpdateObjectActiveStatus(obj);
				}
			}

			ParentGameObjects(columnPool);
		}
	}

	void InstantiateSprings() {
		GameObject obj = null;
		Vector3 location = Vector3.zero;
		Vector3 heading = Vector3.zero;
		Quaternion rotation = Quaternion.identity;

		if(spring != null) {
			for (int i = 0; i < springPoolSize; i++) {
				// Randomly choose a location for the spring
				location = new Vector3 (Random.Range (minSpringX, maxSpringX), 
										Random.Range (minSpringY, maxSpringY),
										 0f);
				// Calculate the vector from field origin to location for rotation quaternion
				heading = location - fieldOrigin.transform.position;
				obj = Instantiate (spring, 
								location, 
								rotation);
				// Check for overlapping GameObjects
				if(CheckOverlap(obj, springPool)) {
					// If so, destroy the new spring
					Destroy(obj);
					// Decrement i to try again
					i--;
				} else {
					// Randomly choose if we are offsetting the rotation of the spring by angleDelta
					StartCoroutine(RotateSkySpring(obj, 
													fieldOrigin, 
													heading, 
													angleDelta));
					// Add the spring to the spring pool
					springPool[i] = obj;
					// Activate the object
					UpdateObjectActiveStatus(obj);
				}
			}

			// Give the springs a parent
			ParentGameObjects(springPool);
		}
	}

	void InstantiateBackground(GameObject backgroundTile, 
							List<GameObject> pool, 
							int poolSize) {
		if(backgroundTile != null) {
			Vector3 location = Vector3.zero;
			Vector3 size = Vector3.zero;

			for(int i = 0; i < poolSize; i++) {
				// If this is the first iteration, then the first background tile starts at Vector3.zero
				if(i == 0) {
					pool[i] = Instantiate(backgroundTile, 
										backgroundTile.transform.position, 
										Quaternion.identity);
				} else {
					// Otherwise, grab the last tile and set the new tile's position
					// to the right side of the last tile.
					GameObject lastTile = pool[i - 1];
					Vector2 s = lastTile.GetComponent<SpriteRenderer>().bounds.size;
					size = new Vector3(s.x, 0f, 0f);
					pool[i] = Instantiate(backgroundTile, 
										lastTile.transform.position + size, 
										Quaternion.identity);
				}

				// Activate the new sky tile
				pool[i].SetActive(true);
			}

			ParentGameObjects(pool);
		}
	}

	void ParentGameObjects(List<GameObject> pool) {
		GameObject dummy = new GameObject();
		// Set each object's name in the pool to it's tag
		foreach(GameObject o in pool) {
			o.transform.SetParent(dummy.transform);
			o.name = o.tag;
		}
		if(pool[0] != null) {
			// If the pool contains sky tiles, add a box collider to the parent
			if(pool[0].tag == "Sky") {
				dummy.AddComponent<BoxCollider2D>();
				dummy.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 56f);
				dummy.GetComponent<BoxCollider2D>().size = new Vector2(500f, 0.5f);
			} else if(pool[0].tag == "Grass") {
				dummy.AddComponent<BoxCollider2D>();
				// dummy.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 56f);
				// dummy.GetComponent<BoxCollider2D>().size = new Vector2(500f, 0.5f);
			}
		}
		dummy.name = pool[0].name + "Holder";
		dummy.SetActive(true);
	}

	void UpdatePlayerScore(UIManagerScript script) {
		// Update the player's final score
		script.UpdateScore();
	}

	void UpdateCanvas(Canvas canvas) {
		// Toggle canvas visibilty
		canvas.enabled = !canvas.enabled;
	}

	//////////////////////////
	// CoRoutines			//
	//////////////////////////


	IEnumerator SetSpringRotation(GameObject obj, 
								GameObject origin, 
								Vector3 heading, 
								float rotOffset) {
		// Randomly choose if we are offsetting the rotation of the spring by rotOffset
		if(obj != null) {
			if(CoinFlip()) {
				// Randomly choose if we are negating the delta
				if(CoinFlip()) {
					rotOffset *= -1f;
				}
				// Rotate the spring with an angle offset
				RotateSkySpring(obj, origin, heading, rotOffset);
			} else {
				// Rotate the spring without an angle offset. We didn't win the coin flip :,(
				RotateSkySpring(obj, origin, heading, 0f);
			}
		}
		yield return null;
	}

	IEnumerator Restart() {
		UIManagerScript.restart = false;
		// Enable the UI Canvas
		UpdateCanvas(UICanvas.GetComponent<Canvas>());
		// Reload the scene
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		yield return new WaitForSeconds(1f);
	}
}
