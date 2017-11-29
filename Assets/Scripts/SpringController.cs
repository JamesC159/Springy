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
    private Rigidbody2D ballRb;
    private Vector3 initialPos;
	private Vector3 initialScale;
    private Vector3 launchHeading;
	private Vector3 cursorPosition;
    private Vector2 F;
    private float x;
    private float launchAngle;
    private bool dragging;

    private void Awake () {
    }

    // Use this for initialization
	private void Start () {
		initialPos = Vector3.zero;
		initialScale = transform.localScale;
		cursorPosition = Vector3.zero;
        F = Vector2.zero;
        x = 0f;
        launchAngle = 0f;
        dragging = false;
    }
	
    // Update is called once per frame
    private void Update () {
       
    }

	private void FixedUpdate() {
		// Rotate the launch spring as long as we are dragging the mouse by launchAngle degrees.
		if (dragging) {
			transform.Rotate (0, 0, launchAngle);
            // The ball is not a child of the spring, update its position relative to the launch spring
            if(ballRb != null) {
                ballRb.transform.RotateAround(transform.position, transform.forward, launchAngle);
            }
			//compress the spring based on the user's mouse position
			if (cursorPosition.y / 0.25f > 0.13f && cursorPosition.y / 0.25f < 0.5f) { 
				transform.localScale = Vector3.MoveTowards (transform.localScale,
					               new Vector3 (initialScale.x, cursorPosition.y / 0.25f, initialScale.z), 0.25f);
			}
		}
        if(ballRb != null) {
            PlayerController p = ballRb.GetComponent<PlayerController>();
            if (p.hasLaunched) {
                if (ballRb.velocity.magnitude > 30) { 
                    // if the ball is going too fast gradually slow it down
                    // putting it at .93 instead of .99 gives it more of a whiffle / beach ball feel
                    ballRb.velocity *= 0.93f;
                }
                transform.localScale = Vector3.MoveTowards (transform.localScale, initialScale, 1);
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            ballRb = collision.collider.gameObject.GetComponent<Rigidbody2D>();
            PlayerController p = ballRb.GetComponent<PlayerController>();
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
        dragging = false;
        if (ballRb != null) {
            PlayerController p = ballRb.GetComponent<PlayerController>();
            // If the ball has not been launched when the user lets go of the mouse, then launch the ball kinematically and allow dynamics to take over.
			if(!p.hasLaunched) {
                ballRb.velocity = CalcVel(x, draggingMass);
                // Cap the max initial speed of the ball so it won't fly off the screen
                // This is a weird way of doing this, but directly editing ballRb.velocity.x/y won't work for whatever reason
                Vector3 velocityLimit = ballRb.velocity;
                velocityLimit.x = Mathf.Clamp (velocityLimit.x, 0, 30);
                velocityLimit.y = Mathf.Clamp (velocityLimit.y, 0, 20);
                ballRb.velocity = velocityLimit;

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

