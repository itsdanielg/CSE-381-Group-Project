using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

    void Update() {
        foreach (Transform child in transform) {
            AudioSource audio = child.gameObject.GetComponent<AudioSource>();
            audio.volume = PlayerPrefs.GetFloat("Music")/100.0f;
        }
    }

}
