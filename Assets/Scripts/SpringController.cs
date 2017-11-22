using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour {

    public GameObject springTop;
    public float k;
    public float draggingMass;
    public float forceDampener;
    public float forceMultiplier;

    private PlayerController p;
    private Rigidbody2D rb2d;
    private Rigidbody2D ballRb;
    private SpriteRenderer renderer;
    private Vector3 initialPos;
    private Vector3 cursorPosition;
    private Vector3 launchHeading;
    private Vector2 F;
    private float x;
    private float launchAngle;
    private bool dragging;
    private bool hasLaunched;
    private bool ballDidCollide;

    private void Awake () {
        rb2d = GetComponent<Rigidbody2D> ();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    private void Start () {
        initialPos = Vector3.zero;
        cursorPosition = Vector3.zero;
        launchHeading = Vector3.zero;
        F = Vector2.zero;
        x = 0f;
        launchAngle = 0f;
        dragging = false;
        hasLaunched = false;
        ballDidCollide = false;
    }
	
    // Update is called once per frame
    private void Update () {
       
    }

    private void FixedUpdate() {
        // Rotate the launch spring as long as we are dragging the mouse by launchAngle degrees.
        if(dragging) {
            transform.Rotate(0, 0, launchAngle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            ballRb = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            p = ballRb.gameObject.GetComponent<PlayerController>(); 
            // If the ball has been launched, then we are adding force to the dynamics of the ball rather than setting it's velocity to a precalculated value
            // For some reason on launch, the ball collides with the launch spring twice, so we need to take care of how many times it has collided with the launch spring
            // before deciding that dynamic physics is in play.
            if (p.collisionCounter > 1 && p.hasLaunched) {
                x = CalcX(ballRb.mass, p.vel.magnitude);
                F = transform.up.normalized * k * x * x;
                ballRb.AddForce(F * forceDampener, ForceMode2D.Impulse);
            } 
        }
    }

    private void OnMouseDown() {
        initialPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialPos.z = 0;
    }

    private void OnMouseDrag() {
        dragging = true;
        // Calculate the angle of launch and launch heading.
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        launchHeading = cursorPosition - initialPos;
        launchAngle = Vector3.SignedAngle(transform.up, -launchHeading, Vector3.forward);
        // Note: dragging mass is the mass of the user's click
        x = CalcX(draggingMass, launchHeading.magnitude);
    }

    private void OnMouseUp() {
        initialPos = Vector3.zero;
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
        return transform.up.normalized * Mathf.Sqrt(k * (x * x) / mass) * forceMultiplier;
    }
}

//https://www.rastating.com/adding-springs-to-2d-platformers-in-unity/
//https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
