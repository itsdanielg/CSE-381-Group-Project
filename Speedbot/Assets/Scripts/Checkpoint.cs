using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour {

    public AudioSource audio;
    public TextMeshProUGUI checkpointText;

    private bool triggered;
    private float fadeRate;
    private float alpha;

    void Start() {
        fadeRate = 10f;
        alpha = 190f;
        checkpointText.color = new Color32(255, 255, 255, 0);
        checkpointText.gameObject.SetActive(false);
    }

    void Update() {
        if (triggered) {
            fadeRate += 1f;
            alpha -= fadeRate * Time.deltaTime;
            checkpointText.color = new Color32(255, 255, 255, (byte)((int)alpha));
            if (alpha <= 0) {
                checkpointText.color = new Color32(255, 255, 255, 0);
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerEnter(Collider player) {
        if (player.CompareTag("Player")) {
            audio.volume = PlayerPrefs.GetFloat("Sound");
            audio.Play();
            PlayerController.respawnPoint = transform.position;
            PlayerController.respawnPoint.y += 2.0f;
            checkpointText.color = new Color32(255, 255, 255, (byte)((int)alpha));
            checkpointText.gameObject.SetActive(true);
            triggered = true;
        }
    }

}
