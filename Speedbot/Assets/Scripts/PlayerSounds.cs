using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public AudioSource stepSound;
    public AudioSource jumpSound;
    public AudioSource doubleJumpSound;

    void step() {
        stepSound.Play();
    }

    void jump() {
        jumpSound.Play();
    }

    void doubleJump() {
        doubleJumpSound.Play();
    }

}
