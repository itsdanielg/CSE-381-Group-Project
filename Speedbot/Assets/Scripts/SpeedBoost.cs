﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {

    public AudioSource audio;

    private float respawnTime = 2f;
    private float speedBoostDuration = 2.0f;
    private float speedBoostMultiplier = 2.2f;

    void Update() {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider player) {
        audio.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
        audio.Play();
        if (player.CompareTag("Player")) {
            StartCoroutine(PickUp(player));
        }
    }

    IEnumerator PickUp(Collider player) {
        PlayerController.boostMultiplier = speedBoostMultiplier;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(speedBoostDuration);
        PlayerController.boostMultiplier = 1.0f;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
    
}
