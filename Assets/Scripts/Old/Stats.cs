using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable] public class OnLevelChange : UnityEngine.Events.UnityEvent<int>{ }
[System.Serializable] public class OnHealthChange : UnityEngine.Events.UnityEvent<int, int>{ }
[System.Serializable] public class OnExpChange : UnityEngine.Events.UnityEvent<int, int>{ }

public class Stats2 : MonoBehaviour {
    public int Level {
        get => lvl;
        set {
            lvl = value;

            if(OnLevelChange != null)   OnLevelChange.Invoke(lvl);
        }
    }
    [SerializeField] private int lvl = 1;

    public int Exp {
        get => exp;
        set {
            exp = value;
            if (OnExpChange != null) OnExpChange.Invoke(value, Level * 100);

            LevelUp();
        }
    }
    [SerializeField] private int exp;

    public int Health { get => this.hp; }
    public int Strength { get => this.strength; }
    public int Defense { get => this.defense; }
    public int Speed { get => this.speed; }
    public int Luck { get => this.luck; }
    public int Accuracy { get => this.accuracy; }

    public int StatsPoints;

    [SerializeField] private int hp = 10;
    [SerializeField] private int strength = 1;
    [SerializeField] private int defense = 1;
    [SerializeField] private int speed = 1;
    [SerializeField] private int luck = 1;
    [SerializeField] private int accuracy = 1;

    private SpriteRenderer renderer;

    public OnLevelChange OnLevelChange;
    public OnHealthChange OnHealthChange;
    public OnExpChange OnExpChange;

    #region HEALTH
    public bool Invulnerable;
    public float InvulTime;
    private float deltaInvul;


    public int CurrentHealth {
        get => this.currentHealth;
        set {
            if(value < 0)   this.currentHealth = 0;
            else            this.currentHealth = value;

            if(OnHealthChange != null)  OnHealthChange.Invoke(this.currentHealth, Health);
        }
    }
    [SerializeField] private int currentHealth = 10;

    public void Heal(int _heal) => this.CurrentHealth += _heal;

    public void Damage(int _damage) {
        CurrentHealth -= _damage * (Invulnerable ? 0 : 1);

        if (currentHealth <= 0) {
            if (this.gameObject.tag.Equals("Enemy")) {
                GameObject
                    .Find("Player")
                    .GetComponent<Stats>()
                    .AddExperience(Exp);

                // questManager.enemyKilled = quest;
            } else if (gameObject.tag.Equals("Player")) {
                //GameOver
            }

            gameObject.SetActive(false);
        }

        if (InvulTime > 0) {
            Invulnerable = true;
            this.deltaInvul = InvulTime;
        }
    }

    Interprivate void Flash(bool _flash) {
        this.renderer.color = new Color(
            this.renderer.color.r,
            this.renderer.color.g,
            this.renderer.color.b,
            (_flash ? 1.0f : 0.0f)
        );
    }
    #endregion

    #region EXP
    public void LevelUp(){
        if(Level * 100 <= Exp) {
            Exp -= Level * 100;
            Level++;

            if(OnLevelChange != null)   OnLevelChange.Invoke(Level);
        }
        if(OnExpChange != null) OnExpChange.Invoke(Exp, Level * 100);
    }
    public void AddExperience(int _exp) {
        Exp += _exp;
        Debug.Log("EXP: " + _exp);
    }
    #endregion


    private void Update() {
        // Attack Inmunity
        if (Invulnerable) {
            this.deltaInvul -= Time.deltaTime;
            if (this.deltaInvul > InvulTime * (2 / 3))
                Flash(false);
            else if (this.deltaInvul > InvulTime / 3)
                Flash(true);
            else if (this.deltaInvul > 0)
                Flash(false);
            else {
                Flash(true);
                Invulnerable = false;
            }
        }
    }

    private void Start() {
        this.renderer = GetComponentInChildren<SpriteRenderer>();

        CurrentHealth = 10;
        Exp = 0;
        Level = 1;
    }
}
*/