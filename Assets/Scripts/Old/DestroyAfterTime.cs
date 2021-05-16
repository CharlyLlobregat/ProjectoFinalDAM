using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float Time;

    private void Update() {
        Time -= UnityEngine.Time.deltaTime;

        if(Time < 0)    Destroy(gameObject);
    }
}