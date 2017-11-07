using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour {

    public float k;
    public float draggingMass;

    private Rigidbody2D rb2d;
    private Rigidbody2D ballRb;
    private Vector3 initialPos;
    private Vector2 ballVel;
    private float x;
    private bool dragging;

    private void Awake () {
        rb2d = GetComponent<Rigidbody2D> ();
        ballRb = null;
    }

    // Use this for initialization
    private void Start () {
        
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
            // Make the ball's rigidbody kinematic so that we can apply Conservation of Energy
            // equations for this spring system.
            ballRb.isKinematic = true;
            ballVel = p.GetVel();
            x = CalcX(ballRb.mass, ballVel.magnitude);
            ballRb.velocity = CalcVel(x, ballRb.mass);
            MakeBallDynamic();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // Set the ball's rigidbody to null so we do not accidentally apply physics to the ball once it has
        // left contact with the spring.
        ballRb = null;
    }

    private void OnMouseDrag() {
        dragging = true;
        // Calculate the spring compress from the magnitude of the vector between initial and current mouse positions
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
        Vector3 heading = cursorPosition - initialPos;
        x = CalcX(draggingMass, heading.magnitude);
    }

    private void OnMouseUp() {
        dragging = false;
        // When the use lets go of the mouse button, calculate the ball's launch velocity
        // from the x derived from OnMouseDrag()
        if (ballRb != null) {
            ballRb.velocity = CalcVel(x, draggingMass);
            MakeBallDynamic();
        }
    }

    private void MakeBallDynamic() {
        if (ballRb.isKinematic) {
            ballRb.isKinematic = false;
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
