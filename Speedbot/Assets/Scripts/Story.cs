using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour {

    private const string MENU = "MainMenu";
    private const string NEXT_LEVEL = "LevelOne";

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LevelChanger.fadeToMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            LevelChanger.fadeToNextLevel(NEXT_LEVEL);
        }
    }

}
