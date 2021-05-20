using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance {get; private set;}

    public Slider PlayerHealthBar;
    public Text PlayerrHealthText;

    public Text PlayerLevelText;

    public Slider PlayerExpBar;
    public Text PlayerExpText;

    public Stats.EntityStats PlayerStats;

    public Canvas DamageCanvas;

    private Color32 baseColor;

    private void Start() {
        Instance = this;
        this.baseColor = new Color(1, 1, 1, 0.433f);
    }

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

    public void ResetWindows() {
        for(int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).GetComponent<WindowController>().Show();
    }

    public void UpdateRed(string _red) {
        _red = _red.Equals(string.Empty) ? "0" : _red;
        this.baseColor.r = byte.TryParse(_red, out byte temp) ? temp : byte.MaxValue;
        UpdateColor(this.baseColor);
    }
    public void UpdateGreen(string _green) {
        _green = _green.Equals(string.Empty) ? "0" : _green;
        this.baseColor.g = byte.TryParse(_green, out byte temp) ? temp : byte.MaxValue;
        UpdateColor(this.baseColor);
    }
    public void UpdateBlue(string _blue) {
        _blue = _blue.Equals(string.Empty) ? "0" : _blue;
        this.baseColor.b = byte.TryParse(_blue, out byte temp) ? temp : byte.MaxValue;
        UpdateColor(this.baseColor);
    }
    public void UpdateColor(Color _rgb) {
        this.baseColor = _rgb;

        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).GetComponent<Image>().color = _rgb;
    }

    public void CreateDamageNumber(Vector3 _position, uint _damage) {
        Instantiate(
            DamageCanvas.gameObject,
            _position,
            Quaternion.Euler(Vector3.zero)
        ).GetComponent<DamageNumber>().DamagePoints = _damage;
    }

    public void StartDialogue(Dialogue.DialogueController _dialogue) {
        Dialogue.DialogueManager.Instance.gameObject.SetActive(true);
        Dialogue.DialogueManager.Instance.CurrentDialogue = _dialogue;
    }
}
