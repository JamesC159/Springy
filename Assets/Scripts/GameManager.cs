using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Singleton Instance
	public static GameManager instance = null;

	// GameObjects
	public List<GameObject> prefabHolder;
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
	List<GameObject> columnPool = new List<GameObject>();
	List<GameObject> springPool = new List<GameObject>();
	List<GameObject> skyPool = new List<GameObject>();
	List<GameObject> grassPool = new List<GameObject>();

	void Awake() {
		// Abide by the Singleton pattern
		if(instance == null) {
			instance = this;
		} else if(instance != this) {
			Destroy(this.gameObject);
			return;
		}
		// Make sure the GameManager doesnt get destroyed between scenes.
		DontDestroyOnLoad(this.gameObject);
		// Initialize object pools
		InitObjectPool(columnPool, columnPoolSize);
		InitObjectPool(springPool, springPoolSize);
		InitObjectPool(skyPool, skyPoolSize);
		InitObjectPool(grassPool, grassPoolSize);
	}

	void RandomizeFieldOrigin(){
		// Randomize field origin point
		Vector3 location = new Vector3(Random.Range(10f, 20f),
									0f,
									0f);
		fieldOrigin.transform.position = location;
	}

	// Use this for initialization
	void Start () {
		// Add prefabs to the prefab holder
		prefabHolder.Add(column);
		prefabHolder.Add(spring);
		prefabHolder.Add(grassTile);
		prefabHolder.Add(skyTile);
		// Activate launch setup and field origin
		UpdateObjectActiveStatus(fieldOrigin);
		UpdateObjectActiveStatus(launchSpring);
		UpdateObjectActiveStatus(ball);
		// Randomize the location of the field origin
		RandomizeFieldOrigin();
		// Instantiate the rest of the prefabs in the world
		InstantiatePrefabs();
	}
	
	// Update is called once per frame
	void Update () {

		// Handle restarting the scene
		if(UIManagerScript.instance.restart) {
			UIManagerScript.instance.restart = false;
			PlayerController.score = 0;
			// Clear object pools
			ClearPool(skyPool, skyPoolSize);
			ClearPool(grassPool, grassPoolSize);
			ClearPool(springPool, springPoolSize);
			ClearPool(columnPool, columnPoolSize);
			// Disable the UI Canvas
			UIManagerScript.instance.DisableCanvas();
			// Reload the scene
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		} else if (UIManagerScript.instance.quit) {
			// Handle quitting the game
			UIManagerScript.instance.quit = false;
			PlayerController.score = 0;
			Application.Quit();
		}

		// Handle live game state
		if(ball != null) {
			// If the user has died
			if (PlayerController.isDead) {
				// Update player's score and enable the UI to restart or quit
				UIManagerScript.instance.UpdateScore();
				UIManagerScript.instance.EnableCanvas();
				// Player died, reset PlayerController.
				PlayerController.isDead = false;
				PlayerController.collisionCounter = 0;
				PlayerController.hasLaunched = false;
			}
		}

		// Cycle grass and sky positions to keep seemless background
		if(CameraController.moveGrassAndSky) {
			// CycleObjectPositions(grassPool);
			// CycleObjectPositions(skyPool);
			CycleObjectPositions(grassPool);
			CameraController.moveGrassAndSky = false;
		}
	}

	/////////////////////////////////////
	// GameObject Instiation Methods.  //
	/////////////////////////////////////

	void InstantiatePrefabs() {
		if(prefabHolder != null) {
			// Instantiate each type of GameObject in the prefabs holder.
			foreach(GameObject child in prefabHolder) {
				switch(child.gameObject.tag) {
					case "Spring":
						InstantiateSprings();
						break;
					case "Column":
						InstantiateColumns();
						break;
					case "Grass":
						InstantiateBackground(grassTile, grassPool, grassPoolSize);
						break;
					case "Sky":
						InstantiateBackground(skyTile, skyPool, skyPoolSize);
						break;
				}
			}
		} else {
			throw new System.NullReferenceException("[-] GameManager::InstantiatePrefabs : " 
												+ "Failed instantiating prefabs because prefabHolder was null");
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

	////////////////////////////////
	// Object Pool Methods        //
	////////////////////////////////

	void InitObjectPool(List<GameObject> pool, int poolSize) {
		GameObject dummy = null;
		for(int i = 0; i < poolSize; i++) {
			pool.Add(dummy);
		}
	}

	void ClearPool(List<GameObject> pool, int poolSize) {
		for(int i = 0; i < poolSize; i++) {
			Destroy(pool[i]);
		}
	}

	void CycleObjectPositions(List<GameObject> pool) {
		GameObject firstObj = null;
		GameObject lastObj = null;
		int idxFirst = -1;
		int idxLast = -1;

		// If there is more than one object in the pool, 
		// then firstObj and lastObj cannot be the same
		if(pool.Count >= 2) {
			// Look for the first non-null object in the pool
			for(int i = 0; i < pool.Count - 1; i++) {
				if(pool[i]) {
					firstObj = pool[i];
					idxFirst = i;
					break;
				}
			}
			// Look for the last non-null object in the pool
			for(int i = pool.Count - 1; i > 0; i--) {
				if(pool[i]) {
					lastObj = pool[i];
					idxLast = i;
					break;
				}
			}
		} else if(pool.Count == 1) {
			// pool.Count < 1 so set firstObj and lastObj 
			// to be equal to the first object in the pool.
			firstObj = lastObj = pool[0];
		} else {
			// There are no GameObjects in the pool.
			print("[-] CycleObjectPositions() : No objects in the pool to cycle through");
		}
		if(firstObj && lastObj) {
			// Get the width and height of the objects in the pool
			Vector2 size = firstObj.GetComponent<SpriteRenderer>().bounds.size;
			Vector3 size3 = new Vector3(size.x, 0f, 0f);
			// Place the first object in the pool to the right of the last object
			firstObj.transform.position = lastObj.transform.position + size3;
			// If lastObj = firstObj, then do not remove it from the pool.
			if(idxFirst > -1 && idxLast > -1) {
				if(idxFirst == idxLast) {
					return;
				} else if(idxLast < pool.Count - 1) {
					// If the last non-null object is not the last in the pool,
					// Insert the first non-null object to the next element after the last.
					pool.Insert(idxLast + 1, firstObj);
				} else if(idxLast == pool.Count - 1) {
					// If the last non-null object is the last object in the pool,
					// add the firstObject to the end of the pool as a new element.
					pool.Add(firstObj);
				} 
			}
			pool.Remove(firstObj);
		}
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
					in obj.transform) {
					child.gameObject.SetActive(true);
				}
			}
		}
	}

	void ParentGameObjects(List<GameObject> pool) {
		GameObject dummy = new GameObject();
		// Set each object's name in the pool to it's tag
		foreach(GameObject o in pool) {
			if(o != null) {
				o.transform.SetParent(dummy.transform);
				o.name = o.tag;
			}
		}
		if(pool[0] != null) {
			// If the pool contains sky tiles, add a box collider to the parent
			if(pool[0].tag == "Sky") {
				dummy.tag = "Sky";
				dummy.AddComponent<BoxCollider2D>();
				dummy.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 56f);
				dummy.GetComponent<BoxCollider2D>().size = new Vector2(500f, 0.5f);
			} else if(pool[0].tag == "Grass") {
				dummy.tag = "Ground";
				dummy.AddComponent<BoxCollider2D>();
				dummy.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -5f);
				dummy.GetComponent<BoxCollider2D>().size = new Vector2(500f, 3.7f);
			}
		}
		dummy.name = pool[0].name + "Holder";
		dummy.SetActive(true);
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
}
