using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour{

  //Button to start a New Game
  public void NewGameBtn(string newGameLevel){
    SceneManager.LoadScene(newGameLevel);
  }
  //Button to exit the game
  pubclic void ExitGameBtn(){
    Application.Quit();
  }

}
