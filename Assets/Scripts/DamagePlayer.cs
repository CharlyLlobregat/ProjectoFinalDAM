using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
    public int Damage;

    public Canvas DamageNumber;

    private void OnCollisionEnter2D(Collision2D _other) {
        if (_other.gameObject.name.Equals("Player")) {
            _other.gameObject.GetComponent<InteractionManager>().TakeDamage(GetComponent<InteractionManager>().Damage());
        }
    }
}
