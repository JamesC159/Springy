using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
 
 public class TimeManager : MonoBehaviour {
 
     public float startingTime;
     private float countingTime;
     private Text message;
     GUIText guiLives;
     public LopterMovement player;
      int lives = 3;
      int score = 0;
     
 
     // initialization
     void Start () {
         countingTime = startingTime;
         message = GetComponent<Text> ();
         player = FindObjectOfType<LopterMovement> ();
     }
     
     // Update is called once per frame
     void Update () {
         countingTime -= Time.deltaTime;
 
         if (countingTime <= 0) {
             Application.LoadLevel(startLevel);
         }
 
         message.text = "" + Mathf.Round (countingTime);
     }
 
 
     public void ResetTime(){
         countingTime = startingTime;
         {
     }
     
     
     public void loseLife(){
     lives--;
     guiLives.text = "Lives: " + lives;
     if(lives > 0){
     SpawnBall();
     }
     else{
     Destroy(gameObject);
     }
    }
 }
