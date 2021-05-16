using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject Target;
    public float CameraSpeed = 5.0f;

    private Vector3 targetPosition;

    void Start() {

    }

    void Update() {
        this.targetPosition = new Vector3(
            Target.transform.position.x,
            Target.transform.position.y,
            this.transform.position.z
        );
    }

    private void LateUpdate() {
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            this.targetPosition,
            Time.deltaTime * CameraSpeed
        );
    }
}
