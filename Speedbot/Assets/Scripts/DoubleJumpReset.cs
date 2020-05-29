using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpReset : MonoBehaviour {

    public AudioSource audio;

    private float respawnTime = 2f;

    void Update() {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider player) {
        audio.volume = PlayerPrefs.GetFloat("Sound");
        audio.Play();
        if (player.CompareTag("Player")) {
            StartCoroutine(PickUp(player));
        }
    }

    IEnumerator PickUp(Collider player) {
        PlayerController.currentJump = 1;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
   
}
