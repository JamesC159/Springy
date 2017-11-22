using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Collider[] colliders;
	public GameObject column;
	public int numColumns;
	public float radius = 0f;

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
		
	}

	void LateUpdate() {
		
	}
}
