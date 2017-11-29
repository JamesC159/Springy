using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {
    public void LoadByIndex (int index) { 
        SceneManager.LoadScene (index);
    }

    public void escape() { 
        Application.Quit ();
    }
}
