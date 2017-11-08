using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int collisionCounter;
    public bool hasLaunched;
    public Vector2 vel;
    private Rigidbody2D rb2d;

    // Use this for initialization
    private void Start () {
		rb2d = GetComponent<Rigidbody2D>();
        hasLaunched = false;
        collisionCounter = 0;
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
