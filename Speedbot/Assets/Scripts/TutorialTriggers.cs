using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialTriggers : MonoBehaviour {

    private const string MENU = "MainMenu";
    private const string NEXT_LEVEL = "Cutscene";

    public GameObject triggers;
    public GameObject textTriggers;
    private RaycastHit hit;
    public Camera camera;
    public CharacterController controller;
    public TextMeshProUGUI timer;
    public GameObject completeLevel;

    private int triggerIndex;
    private int textTriggerIndex;
    private bool switchMode;
    private IEnumerator waitMessage;
    private Vector3 respawnPoint;
    private Vector3 cameraSpawn;
    

    private float currentTime;
    
    // Start is called before the first frame update
    void Start() {
        cameraSpawn = new Vector3(0, 4, -10);
        triggerIndex = 0;
        textTriggerIndex = 0;
        switchMode = false;
        initText();
        textTriggers.transform.GetChild(0).gameObject.SetActive(true);
        respawnPoint = new Vector3(0, 5, 0);
        waitMessage = waitText();
        timer.gameObject.SetActive(false);
        completeLevel.SetActive(false);
        currentTime = 67.31f;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(MENU);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(NEXT_LEVEL);
        }
        respawnTrigger();
        switch(textTriggerIndex) {
            case 0:
                if (Vector3.Distance(camera.transform.position, cameraSpawn) > 12) {
                    displayText();
                }
                break;
            case 1:
                checkTrigger();
                break;
            case 2:
                checkTrigger();
                break;
            case 3:
                checkTrigger();
                break;
            case 4:
                checkTrigger();
                break;
            case 5:
                checkTrigger();
                break;
            case 6:
                textTriggerIndex++;
                StartCoroutine(waitText());
                break;
            case 7:
                if (Input.GetMouseButtonDown(1)) {
                    displayText();
                }
                break;
            case 8:
                if (Input.GetMouseButtonDown(0)) {
                    if (PlayerController.hookPlaced) {
                        displayText();
                    }
                }
                break;
            case 9:
                checkTrigger();
                break;
            case 10:
                checkTrigger();
                break;
            case 11:
                checkTrigger();
                break;
            case 12:
                checkTrigger();
                break;
            case 13:
                checkTrigger();
                break;
            case 14:
                updateTimer();
                if (currentTime <= 0) {
                    currentTime = 0;
                    textTriggerIndex += 2;
                    textTriggers.transform.GetChild(textTriggerIndex).gameObject.SetActive(true);
                    textTriggers.transform.GetChild(textTriggerIndex-2).gameObject.SetActive(false);
                }
                checkTrigger();
                break;
            default:
                break;
        }
    }

    void respawnTrigger() {
        if (controller.transform.position.y < -30) {
            PlayerController.respawnPlayer(respawnPoint);
        }
    }

    void initText() {
        foreach (Transform child in textTriggers.transform) {
            child.gameObject.SetActive(false);
        }
    }

    void updateTimer() {
        currentTime -= Time.deltaTime;
        timer.text = "TIME REMAINING: " + currentTime.ToString("F");
    }

    void checkTrigger() {
        Transform trigger = triggers.transform.GetChild(triggerIndex);
        if (!trigger.gameObject.activeSelf) {
            triggerIndex++;
            displayText();
            respawnPoint = trigger.position;
            respawnPoint.y += 5;
        }
        if (textTriggerIndex == 14 && !switchMode) {
            switchMode = true;
            timer.gameObject.SetActive(true);
            completeLevel.SetActive(true);
        }
    }

    void displayText() {
        textTriggerIndex++;
        textTriggers.transform.GetChild(textTriggerIndex).gameObject.SetActive(true);
        textTriggers.transform.GetChild(textTriggerIndex-1).gameObject.SetActive(false);
    }

    IEnumerator waitText() {
        yield return new WaitForSeconds(2.5f);
        
        textTriggers.transform.GetChild(textTriggerIndex).gameObject.SetActive(true);
        textTriggers.transform.GetChild(textTriggerIndex-1).gameObject.SetActive(false);
    }

    void gameOver() {

    }

}
