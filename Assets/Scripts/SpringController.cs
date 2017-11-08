using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour {

    public float k;
    public float draggingMass;
    public float forceDampener;

    private Rigidbody2D rb2d;
    private Rigidbody2D ballRb;
    private Vector3 initialPos;
    private Vector2 ballVel;
    private float x;
    private bool dragging;
    private bool hasLaunched;

    private void Awake () {
        rb2d = GetComponent<Rigidbody2D> ();
        ballRb = null;
    }

    // Use this for initialization
    private void Start () {
        dragging = false;
        hasLaunched = false;
    }
	
    // Update is called once per frame
    private void Update () {
       
    }

    private void FixedUpdate() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            ballRb = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            PlayerController p = ballRb.gameObject.GetComponent<PlayerController>();
            // If the ball has been launched, then we are adding force to the dynamics of the ball rather than setting it's velocity to a precalculated value
            // For some reason on launch, the ball collides with the launch spring twice, so we need to take care of how many times it has collided with the launch spring
            // before deciding that dynamic physics is in play.
            if (p.collisionCounter > 2 && p.hasLaunched) {
                x = CalcX(ballRb.mass, p.vel.magnitude);
                Vector2 F = transform.up.normalized * k * x * x;
                ballRb.AddForce(F * forceDampener, ForceMode2D.Impulse);
            } 
        }
    }

    private void OnMouseDrag() {
        dragging = true;
        // Calculate the spring compression from the magnitude of the vector between initial and current mouse positions
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
        Vector3 heading = cursorPosition - initialPos;
        // Note: dragging mass is the mass of the user's click
        x = CalcX(draggingMass, heading.magnitude);
    }

    private void OnMouseUp() {
        dragging = false;
        if (ballRb != null) {
            PlayerController p = ballRb.gameObject.GetComponent<PlayerController>();
            // If the ball has not been launched when the user lets go of the mouse, then launch the ball kinematically and allow dynamics to take over.
            if(!p.hasLaunched) {
                ballRb.velocity = CalcVel(x, draggingMass);
                p.MakeDynamic();
                p.hasLaunched = true;
            }
        }
    }


    // Conservation of Energy equations

    private float CalcX(float mass, float vel) {
        return Mathf.Sqrt(mass * (vel * vel) / k);
    }

    private Vector3 CalcVel(float x, float mass) {
        return transform.up.normalized * Mathf.Sqrt(k * (x * x) / mass);
    }
}

//https://www.rastating.com/adding-springs-to-2d-platformers-in-unity/
//https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
