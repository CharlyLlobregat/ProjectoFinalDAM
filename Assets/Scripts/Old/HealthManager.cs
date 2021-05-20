using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class HealthManager : MonoBehaviour {
    public int MaxHealth = 5;

    [SerializeField]
    private int currentHealth;
    public int CurrentHealth {
        get => this.currentHealth;
        set {
            if(value < 0)   this.currentHealth = 0;
            else            this.currentHealth = value;
        }
    }

    public bool Invulnerable;
    public float InvulTime;
    private float deltaInvul;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        CurrentHealth = MaxHealth;
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Damage(int _damage) {
        CurrentHealth -= _damage;

        if(currentHealth <= 0) {
            if (gameObject.CompareTag("Enemy")) {
                GameObject
                    .Find("Player")
                    .GetComponent<Stats.EntityStats>()
                    .AddExp(gameObject.GetComponent<Stats.EntityStats>().EXP);

                // questManager.enemyKilled = quest;
            }else if (gameObject.CompareTag("Player")) {
                //GameOver
            }

            gameObject.SetActive(false);
        }

        if(InvulTime > 0) {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<PlayerController>().CanMove = false;
            Invulnerable = true;
            this.deltaInvul = InvulTime;
        }
    }

    public void UpdateMaxHealth(int _maxHealth) {
        this.MaxHealth = _maxHealth;
        CurrentHealth = _maxHealth;
    }


    private void Flash(bool _flash) {
        this.spriteRenderer.color = new Color(
            this.spriteRenderer.color.r,
            this.spriteRenderer.color.g,
            this.spriteRenderer.color.b,
            (_flash ?  1.0f : 0.0f)
        );
    }
    private void Update() {
        if (Invulnerable) {
            this.deltaInvul -= Time.deltaTime;
            if(this.deltaInvul > InvulTime * (2/3))
                Flash(false);
            else if(this.deltaInvul > InvulTime / 3)
                Flash(true);
            else if(this.deltaInvul > 0)
                Flash(false);
            else {
                Flash(true);
                Invulnerable = false;

                GetComponent<Collider2D>().enabled = true;
                GetComponent<PlayerController>().CanMove = true;
            }
        }
    }
}
*/