using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb2d;
    float speed;	

    // Use this for initialization
    void Start () {
		rb2d = GetComponent<Rigidbody>();
    }
	
    // Update is called once per frame
    void Update () {
		float move_Horizontal = Input.GetAxis("Horizontal"); 
		float move_Vertical = Input.GetAxis("Vertical");
	    	Vector2 movement = new Vector2(move_Horizontal, move_Vertical);
	    	rb2d.AddForce(movement * speed);
    }
}
