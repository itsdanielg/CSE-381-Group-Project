using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour {

    public AudioSource hoverSound;
    public AudioSource clickSound;

    public void playHover() {
        hoverSound.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
        hoverSound.Play();
    }

    public void playClick() {
        clickSound.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
        clickSound.Play();
    }

}
