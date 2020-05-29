using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float maxDistanceTravel;
    public float travelSpeed;
    public bool moveX;
    public bool moveY;
    public bool moveZ;
    public bool randomPosition;

    private Vector3 spawnPos;
    private Vector3 middlePos;
    private Vector3 firstPos;
    private Vector3 lastPos;
    private CharacterController controller;
    private Rigidbody rigidbody;

    private bool movingFirst;
    private bool movingLast;

    void Start() {
        controller = null;
        rigidbody = GetComponent<Rigidbody>();
        firstPos = lastPos = middlePos = transform.position;
        if (moveX) {
            firstPos.x -= maxDistanceTravel/2;
            lastPos.x += maxDistanceTravel/2;
        }
        if (moveY) {
            firstPos.y -= maxDistanceTravel/2;
            lastPos.y += maxDistanceTravel/2;
        }
        if (moveZ) {
            firstPos.z -= maxDistanceTravel/2;
            lastPos.z += maxDistanceTravel/2;
        }
        movingFirst = false;
        movingLast = true;
        if (randomPosition) {
            float x = Random.Range(firstPos.x, lastPos.x);
            float y = Random.Range(firstPos.y, lastPos.y);
            float z = Random.Range(firstPos.z, lastPos.z);
            spawnPos = new Vector3(x, y, z);
        }
        else {
            spawnPos = middlePos;
        }
        transform.position = spawnPos;
    }

    void FixedUpdate() {
        Vector3 currentPos = transform.position;
        Vector3 movePos = lastPos - firstPos;
        if (movingFirst) {
            movePos = -(movePos);
            movePos = movePos.normalized * travelSpeed * Time.deltaTime;
            rigidbody.MovePosition(currentPos + movePos);
            if (Vector3.Distance(transform.position, lastPos) >= maxDistanceTravel) {
                movingLast = true;
                movingFirst = false;
            }
        }
        else {
            movePos = movePos.normalized * travelSpeed * Time.deltaTime;
            rigidbody.MovePosition(currentPos + movePos);
            if (Vector3.Distance(transform.position, firstPos) >= maxDistanceTravel) {
                movingLast = false;
                movingFirst = true;
            }
        }
        
    }

    void OnTriggerEnter(Collider player) {
        if (player.CompareTag("Player")) {
            controller = player.GetComponent<CharacterController>();
        }
    }

    void OnTriggerStay(Collider player) {
        if (player.CompareTag("Player")) {
            PlayerController.currentJump = 0;
            Vector3 vel = transform.GetComponent<Rigidbody>().velocity;
            controller.Move(vel * Time.deltaTime);
        }
    }

}
