using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance {get; private set;}
    private void Start() => Instance = this;

    public Slider PlayerHealthBar;
    public Text PlayerrHealthText;

    public Text PlayerLevelText;

    public Slider PlayerExpBar;
    public Text PlayerExpText;

    public Stats.EntityStats PlayerStats;
    // private WeaponManager weaponManager;
    // private ItemsManager itemsManager;

    public void UpdateExp(uint _currentEXP, uint _maxEXP) {
        System.Text.StringBuilder builder = new StringBuilder()
            .Append("EXP: ")
            .Append(_currentEXP)
            .Append(" / ")
            .Append(_maxEXP);

        PlayerExpBar.maxValue = _maxEXP;
        PlayerExpBar.value = _currentEXP;
        PlayerExpText.text = builder.ToString();
    }

    public void UpdateLevel(uint _level) {
        System.Text.StringBuilder builder = new StringBuilder()
            .Append("Nivel: ")
            .Append(_level);

        PlayerLevelText.text = builder.ToString();
    }

    public void UpdateHealth(uint _currentHealth, uint _maxHealth) {
        PlayerHealthBar.maxValue = _maxHealth;
        PlayerHealthBar.value = _currentHealth;

        System.Text.StringBuilder builder = new System.Text.StringBuilder()
            .Append("HP: ")
            .Append(_currentHealth)
            .Append(" / ")
            .Append(_maxHealth);

        PlayerrHealthText.text = builder.ToString();
    }

    public void ItemUI(Stats.ItemStats _item) {
        GameObject itemUI = this.transform.Find("ItemWindow").Find("Content").Find("Viewport").Find("Content").gameObject;
        itemUI.GetComponentInChildren<Image>().sprite = _item.gameObject.GetComponent<SpriteRenderer>().sprite;
        GameObject stats = itemUI.transform.Find("ItemStats").gameObject;
        StringBuilder builder = new StringBuilder();
        stats.transform.Find("Name").GetComponent<Text>().text = builder.Append("Name: ").Append(_item.Name).ToString();        builder.Clear();
        stats.transform.Find("Level").GetComponent<Text>().text = builder.Append("Level: ").Append(_item.Level).ToString();       builder.Clear();
        stats.transform.Find("Defense").GetComponent<Text>().text = builder.Append("Defense: ").Append(_item.Defense).ToString();     builder.Clear();
        stats.transform.Find("Strength").GetComponent<Text>().text = builder.Append("Strength: ").Append(_item.Strength).ToString();      builder.Clear();
        stats.transform.Find("Speed").GetComponent<Text>().text = builder.Append("Speed: ").Append(_item.Speed).ToString();       builder.Clear();

        this.transform.Find("ItemWindow").GetComponent<WindowController>().Show();
    }
}
