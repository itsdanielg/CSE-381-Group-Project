using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    void OnTriggerEnter(Collider player) {
        if (player.CompareTag("Player")) {
            PlayerController.isDead = true;
        }
    }


}
