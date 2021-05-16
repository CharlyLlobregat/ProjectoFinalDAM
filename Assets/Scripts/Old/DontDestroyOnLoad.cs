using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    private void Start() {
        if(!PlayerController.PlayerCreated) DontDestroyOnLoad(this.transform.gameObject);
        else                                Destroy(gameObject);
    }
}