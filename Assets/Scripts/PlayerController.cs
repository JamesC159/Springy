using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public int collisionCounter;
    [HideInInspector]
    public bool hasLaunched;
    [HideInInspector]
    public Vector2 vel;
	[HideInInspector]
	public int score;

	private Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    private void Start () {
        collisionCounter = 0;
		score = 0;
        hasLaunched = false;
		isDead = false;
        vel = rb2d.velocity;
    }

    // Update is called once per frame
    private void Update () {
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

    private void OnMouseDown() {
        
    }

    public bool IsDead() {
        return isDead;
    }

    public void SetDead(bool dead) {
        isDead = dead;
    }

    public int GetCollisionCounter() {
        return collisionCounter;
    }

    public void SetCollisionCounter(int count) {
        collisionCounter = count;
    }

    public bool HasLaunched() {
        return hasLaunched;
    }

    public void SetHasLaunched(bool launched) {
        hasLaunched = launched;
    }

    public Vector2 GetVelocity() {
        return vel;
    }

    public void SetVelocity(Vector2 velocity) {
        vel = velocity;
    }

    public void MakeDynamic() {
        if (rb2d != null) {
            if (rb2d.isKinematic) {
                rb2d.isKinematic = false;
            }
            if (rb2d.freezeRotation) {
                rb2d.freezeRotation = false;
            } 
        }
    }
}
