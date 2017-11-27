using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    [HideInInspector]
    public static bool isDead;
    [HideInInspector]
    public static int collisionCounter;
    [HideInInspector]
    public static bool hasLaunched;
    [HideInInspector]
    public Vector2 vel;
	[HideInInspector]
	public static int score;

	Rigidbody2D rb2d;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        collisionCounter = 0;
		score = 0;
        hasLaunched = false;
		isDead = false;
        vel = rb2d.velocity;
    }

    // Update is called once per frame
    void Update () {
        if (rb2d.velocity.sqrMagnitude > 0) {
            vel = rb2d.velocity;
        }
        // Make sure the ball is governed by kinematics and is not moving before launch
        if(!hasLaunched) { 
            rb2d.isKinematic = true;
            rb2d.freezeRotation = true;
            rb2d.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collisionCounter++;
		// If the player collides with the ground, kill the player
		if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Sky") {
			isDead = true;
		} else if (collision.gameObject.tag == "Spring") {
			score++;
		}
    }
}
