using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTwo : MonoBehaviour {

    // CONSTANTS CAN BE CHANGED DEPENDING ON LEVEL
    private const string NEXT_LEVEL = "LevelThree";
    private const float LEVEL_TIME = 100.0f;
    private const float OUT_OF_BOUNDS_DEPTH = -30f;

    // EVERYTHING ELSE REMAINS THE SAME FOR EACH LEVEL

    public GameObject checkpoints;
    public CharacterController controller;
    public AudioSource defeat;

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
        OptionInputs.NEXT_LEVEL = NEXT_LEVEL;
    }

    void Update() {
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
        if (OptionInputs.gamePaused) return;
        currentTime -= Time.deltaTime;
        timerText.text = "TIME REMAINING: " + currentTime.ToString("F");
        if (currentTime <= 0) {
            levelComplete = true;
        }
    }

    void levelCompleted() {
        controller.gameObject.SetActive(false);
    }

    void gameOver() {
        if (controller.gameObject.activeSelf) {
            defeat.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
            defeat.Play();
        }
        controller.gameObject.SetActive(false);
        gameoverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
    }

}
