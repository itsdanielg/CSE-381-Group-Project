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
    private bool movingFirst;
    private bool movingLast;

    void Start() {
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

    void Update() {
        float step = travelSpeed * Time.deltaTime;
        if (movingFirst) {
            transform.position = Vector3.MoveTowards(transform.position, firstPos, step);
            if (Vector3.Distance(transform.position, middlePos) >= maxDistanceTravel/2) {
                movingLast = true;
                movingFirst = false;
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, lastPos, step);
            if (Vector3.Distance(transform.position, middlePos) >= maxDistanceTravel/2) {
                movingLast = false;
                movingFirst = true;
            }
        }
    }

}
