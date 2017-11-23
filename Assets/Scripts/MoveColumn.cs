using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveColumn : MonoBehaviour {

	public float maxDistance;
	public float minDistance;

	private float distance;

	// Use this for initialization
	void Start () {
		distance = Random.Range(minDistance, maxDistance);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
