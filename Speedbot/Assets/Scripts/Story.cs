using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour {

    private const string MENU = "MainMenu";
    private const string NEXT_LEVEL = "LevelOne";

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(MENU);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(NEXT_LEVEL);
        }
    }

}
