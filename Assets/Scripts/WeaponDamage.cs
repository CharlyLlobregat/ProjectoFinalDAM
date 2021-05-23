using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {
    public GameObject BloodAnim;

    public float deltaTimeInside;

    private void OnTriggerEnter2D(Collider2D _other) {
        if (this.transform.parent.GetComponent<Interaction.InteractionController>().IsAttacking && _other.GetComponent<Interaction.InteractionController>().CanBeAttacked && _other.transform != this.transform.parent) {
            if (BloodAnim != null) {
                Destroy(
                    Instantiate(
                        BloodAnim,
                        _other.transform.position,
                        _other.transform.rotation
                    ),
                    0.5f
                );
            }
        }
    }
}