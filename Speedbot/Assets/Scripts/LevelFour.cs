using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelFour : MonoBehaviour {

    // CONSTANTS CAN BE CHANGED DEPENDING ON LEVEL
    private const float LEVEL_TIME = 300.0f;

    // EVERYTHING ELSE REMAINS THE SAME FOR EACH LEVEL

    public GameObject checkpoints;
    public CharacterController controller;
    public AudioSource defeat;
    public AudioSource victory;
    public AudioSource bgm;

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
        OptionInputs.NEXT_LEVEL = "LevelFour";
    }

    void Update() {
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

    void updateTimer() {
        if (OptionInputs.gamePaused) return;
        currentTime -= Time.deltaTime;
        timerText.text = "TIME REMAINING: " + currentTime.ToString("F");
        if (currentTime <= 0) {
            levelComplete = true;
        }
    }

    void levelCompleted() {
        if (controller.gameObject.activeSelf) {
            bgm.Stop();
            victory.volume = PlayerPrefs.GetFloat("Music")/100.0f;
            victory.Play();
        }
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
