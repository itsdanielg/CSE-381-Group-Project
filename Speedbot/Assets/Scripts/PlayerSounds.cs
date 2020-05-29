using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public AudioSource stepSound;
    public AudioSource jumpSound;
    public AudioSource doubleJumpSound;

    void step() {
        stepSound.volume = PlayerPrefs.GetFloat("Sound");
        stepSound.Play();
    }

    void jump() {
        jumpSound.volume = PlayerPrefs.GetFloat("Sound");
        jumpSound.Play();
    }

    void doubleJump() {
        doubleJumpSound.volume = PlayerPrefs.GetFloat("Sound");
        doubleJumpSound.Play();
    }

}
