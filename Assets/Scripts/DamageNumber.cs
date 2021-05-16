using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour {
    public float DamageSpeed;
    public float DamagePoints;

    public Text DamageText;

    private void Update() {

        if(DamageText != null)
            DamageText.text = "" + DamagePoints;

        this.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y + DamageSpeed * Time.deltaTime,
            this.transform.position.z
        );

        this.transform.localScale = this.transform.localScale * (1 - Time.deltaTime / 5);
    }
}