using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionInputs : MonoBehaviour {

    public static bool gamePaused;
    public static string NEXT_LEVEL = "";
    public GameObject optionsMenu;

    void Start() {
        gamePaused = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            gamePaused = !gamePaused;
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            LevelChanger.fadeToNextLevel(NEXT_LEVEL);
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            LevelChanger.fadeToMenu();
        }
    }

}
