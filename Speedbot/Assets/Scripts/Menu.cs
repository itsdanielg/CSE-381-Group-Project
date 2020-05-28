using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private float music = 100.0f;
    private float sound = 100.0f;
    private float mouseSensitivity = 1.5f;

    private const string LEVEL_ONE = "LevelOne";
    private const string LEVEL_TWO = "LevelTwo";
    private const string LEVEL_THREE = "LevelThree";
    private const string LEVEL_FOUR = "LevelFour";

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (!PlayerPrefs.HasKey("Music")) {
            PlayerPrefs.SetFloat("Music", music);
        }
        if (!PlayerPrefs.HasKey("Sound")) {
            PlayerPrefs.SetFloat("Sound", sound);
        }
        if (!PlayerPrefs.HasKey("Sensitivity")) {
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
        }
    }

    public void StartGame() {
        LevelChanger.fadeToNextLevel("Tutorial");
    }

    public void StartLevelOne() {
        LevelChanger.fadeToNextLevel(LEVEL_ONE);
    }
    
    public void StartLevelTwo() {
        LevelChanger.fadeToNextLevel(LEVEL_TWO);
    }

    public void StartLevelThree() {
        LevelChanger.fadeToNextLevel(LEVEL_THREE);
    }

    public void StartLevelFour() {
        LevelChanger.fadeToNextLevel(LEVEL_FOUR);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
