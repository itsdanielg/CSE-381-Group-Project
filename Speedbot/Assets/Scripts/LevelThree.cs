using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelThree : MonoBehaviour {

    // CONSTANTS CAN BE CHANGED DEPENDING ON LEVEL
    private const string MENU = "MainMenu";
    private const string CURRENT_LEVEL = "LevelThree";
    private const string NEXT_LEVEL = "LevelFour";
    private const float LEVEL_TIME = 80.0f;
    private const float OUT_OF_BOUNDS_DEPTH = -30f;

    // EVERYTHING ELSE REMAINS THE SAME FOR EACH LEVEL

    public GameObject checkpoints;
    public CharacterController controller;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameoverText;
    public TextMeshProUGUI restartText;

    public static bool levelComplete;
    private float currentTime;
    
    void Start() {
        // RESPAWN CAN BE CHANGED DEPENDING ON LEVEL
        PlayerController.respawnPoint = new Vector3(0, 5, 0);
        gameoverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        levelComplete = false;
        currentTime = LEVEL_TIME;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(MENU);
        }
        respawnTrigger();
        if (!levelComplete) {
            updateTimer();
        }
        else {
            if (currentTime <= 0) {
                gameOver();
            }
            else {
                levelCompleted();
            }
        }
    }

    void respawnTrigger() {
        if (controller.transform.position.y < OUT_OF_BOUNDS_DEPTH) {
            PlayerController.respawnPlayer();
        }
    }

    void updateTimer() {
        currentTime -= Time.deltaTime;
        timerText.text = "TIME REMAINING: " + currentTime.ToString("F");
        if (currentTime <= 0) {
            levelComplete = true;
        }
    }

    void levelCompleted() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(NEXT_LEVEL);
        }
    }

    void gameOver() {
        gameoverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(CURRENT_LEVEL);
        }
    }

}
