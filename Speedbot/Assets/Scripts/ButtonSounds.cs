using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour {

    public AudioSource hoverSound;
    public AudioSource clickSound;

    public void playHover() {
        hoverSound.Play();
    }

    public void playClick() {
        clickSound.Play();
    }

}
