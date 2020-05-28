using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

    private Vector3 initPos;
    private Vector3 initRotate;
    private Vector3 switchPos;
    private Vector3 switchRotate;
    private float moveTime = 0.6f;

    void Start() {
        initPos = new Vector3(3.25f, 26.25f, -3.7f);
        initRotate = new Vector3(0, -16.85f, 0);
        switchPos = new Vector3(-14.0f, 30.0f, 5.8f);
        switchRotate = new Vector3(1.3f, -107.0f, 0);
        transform.localPosition = initPos;
        transform.localRotation = Quaternion.Euler(initRotate);
    }

    public IEnumerator MoveToPosition(Vector3 fromPos, Vector3 fromRotate, Vector3 toPos, Vector3 toRotate, float time) {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            float a = elapsedTime / time;
            transform.localPosition = Vector3.Lerp(fromPos, toPos, a);
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(fromRotate), Quaternion.Euler(toRotate), a);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
    }

    public void moveToLevel() {
        StartCoroutine(MoveToPosition(initPos, initRotate, switchPos, switchRotate, moveTime));
    }

    public void moveToMenu() {
        StartCoroutine(MoveToPosition(switchPos, switchRotate, initPos, initRotate, moveTime));
    }

}
