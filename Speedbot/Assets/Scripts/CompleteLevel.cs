using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompleteLevel : MonoBehaviour {

    public AudioSource audio;

    public TextMeshProUGUI completeText;
    public TextMeshProUGUI continueText;

    // Start is called before the first frame update
    void Start() {
        completeText.gameObject.SetActive(false);
        continueText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider player) {
        if (player.CompareTag("Player")) {
            audio.Play();
            completeText.gameObject.SetActive(true);
            continueText.gameObject.SetActive(true);
            LevelOne.levelComplete = true;
            LevelTwo.levelComplete = true;
            LevelThree.levelComplete = true;
            LevelFour.levelComplete = true;
        }
    }
}
