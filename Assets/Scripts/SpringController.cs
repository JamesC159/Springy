using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour {
    
    GameObject player;
    Rigidbody2D rb2d;
    float k;
    float distancePulled;
    float launchForce;

    void Awake () {
        player = GameObject.FindGameObjectWithTag ("Player");
        rb2d = GetComponent<Rigidbody2D> ();
    }

    // Use this for initialization
    void Start () {

    }
	
    // Update is called once per frame
    void Update () {
		
    }

    void OnCollisionEnter2D (Collision2D c) {
        Rigidbody2D obj = c.collider.attachedRigidbody;
        if (obj.Equals (player.GetComponent<Rigidbody2D> ())) {
            // Use this to make ball bounce
        }
    }
}
