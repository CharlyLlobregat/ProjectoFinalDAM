using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {
    public int Damage;

    public string WeaponName;

    public GameObject BloodAnim;
    public Canvas DamageNumber;
    public GameObject HitPoint;

    public float deltaTimeInside;

    private void Start() {
        HitPoint = transform.Find("Sword").Find("HitPoint").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.gameObject.tag.Equals("Enemy") && PlayerController.GetInstance().IsAttaking) {

            if (BloodAnim != null && HitPoint != null) {
                Destroy(Instantiate(BloodAnim,
                            HitPoint.transform.position,
                            HitPoint.transform.rotation), 0.5f);
            }

            _other.GetComponent<InteractionManager>().TakeDamage(GameObject.Find("Player").GetComponent<InteractionManager>().Damage());
        }
    }

    private bool oneTime = false;
    private void OnTriggerStay2D(Collider2D _other) {
        if(PlayerController.GetInstance().IsAttaking && !this.oneTime) {
            OnTriggerEnter2D(_other);
            this.oneTime = true;
        }

        if(!PlayerController.GetInstance().IsAttaking)  this.oneTime = false;
    }
}