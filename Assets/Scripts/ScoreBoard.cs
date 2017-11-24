using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
    public GUIText scoreBoard;
    public int score;
    
    void Update() {
        string scoreText = "Score: " + score.ToString();
        scoreBoard.text = scoreText;
    }
}



