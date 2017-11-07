using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 vel;
    private Rigidbody2D rb2d;

    // Use this for initialization
    private void Start () {
		rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update () {
        if (rb2d.velocity.sqrMagnitude > 0) {
            vel = rb2d.velocity;
        }
    }

    private void FixedUpdate() {
        
    }

    public Vector2 GetVel() {
        return vel;
    }
}
