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
	//Rajal -Researched on how to make the ball bounce when it comes in contact with a spring
		/*
		void OnCollisionEnter2D(Collision2D c){
		Rigidbody2D rBody = collisionInfo.collider.GetComponent<Rigidbody2D>();
		Vector2 vel = rBody.velocity;
       		float mag = vel.magnitude;
        	rBody.AddForce(Vector2.Reflect(vel, collisionInfo.contacts[0].normal) * mag, ForceMode2D.Impulse);
		}
		*/
	    //Make sure to uncheck the BoxCollider2D 
    }
}

//https://www.rastating.com/adding-springs-to-2d-platformers-in-unity/
//https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
