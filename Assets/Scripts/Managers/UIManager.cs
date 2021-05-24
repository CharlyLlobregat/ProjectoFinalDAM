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

    public Canvas DamageCanvas;

    private Color32 baseColor;

    public bool OnUIClick;

    public WindowController StatWindow;
    public WindowController InventoryWindow;
    public WindowController ItemWindow;
    public WindowController DialogueWindow;
    public WindowController SettingsWindow;
    public WindowController PauseMenu;
    public WindowController MainMenu;
    public WindowController GameOver;
    public WindowController Credits;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        this.baseColor = new Color(1, 1, 1, 0.433f);
        for (int i = 0; i < this.transform.childCount; i++)
            if (!(
                this.transform.GetChild(i).name == "MainMenu" ||
                this.transform.GetChild(i).name == "PauseMenu"))
                if (this.transform.GetChild(i).TryGetComponent<WindowController>(out WindowController win)) win.OnHide();

        if(!System.IO.File.Exists(Application.persistentDataPath + "/save.dat"))
            this.transform.Find("MainMenu").Find("StartBtn").gameObject.SetActive(false);
    }

    public void UpdateExp(uint _currentEXP, uint _maxEXP) {
        System.Text.StringBuilder builder = new StringBuilder()
            .Append("EXP: ")
            .Append(_currentEXP)
            .Append(" / ")
            .Append(_maxEXP);

        Instance.PlayerExpBar.maxValue = _maxEXP;
        Instance.PlayerExpBar.value = _currentEXP;
        Instance.PlayerExpText.text = builder.ToString();
    }

    public void UpdateLevel(uint _level) {
        System.Text.StringBuilder builder = new StringBuilder()
            .Append("Nivel: ")
            .Append(_level);

        Instance.PlayerLevelText.text = builder.ToString();
    }

    public void UpdateHealth(uint _currentHealth, uint _maxHealth) {
        Instance.PlayerHealthBar.maxValue = _maxHealth;
        Instance.PlayerHealthBar.value = _currentHealth;

        System.Text.StringBuilder builder = new System.Text.StringBuilder()
            .Append("HP: ")
            .Append(_currentHealth)
            .Append(" / ")
            .Append(_maxHealth);

        Instance.PlayerrHealthText.text = builder.ToString();
    }

    public void ItemUI(Stats.ItemStats _item) {
        GameObject itemUI = Instance.transform.Find("ItemWindow").Find("Content").Find("Viewport").Find("Content").gameObject;
        itemUI.GetComponentInChildren<Image>().sprite = Inventory.ItemManager.Instance.GetItem(_item).GetComponent<SpriteRenderer>().sprite;
        GameObject stats = itemUI.transform.Find("ItemStats").gameObject;
        StringBuilder builder = new StringBuilder();
        stats.transform.Find("Name").GetComponent<Text>().text = builder.Append("Name: ").Append(_item.Name).ToString();        builder.Clear();
        stats.transform.Find("Level").GetComponent<Text>().text = builder.Append("Level: ").Append(_item.Level).ToString();       builder.Clear();
        stats.transform.Find("Defense").GetComponent<Text>().text = builder.Append("Defense: ").Append(_item.Defense).ToString();     builder.Clear();
        stats.transform.Find("Strength").GetComponent<Text>().text = builder.Append("Strength: ").Append(_item.Strength).ToString();      builder.Clear();
        stats.transform.Find("Speed").GetComponent<Text>().text = builder.Append("Speed: ").Append(_item.Speed).ToString();       builder.Clear();

        Instance.transform.Find("ItemWindow").GetComponent<WindowController>().Show();
    }

    public void ResetWindows() {
        for(int i = 0; i < this.transform.childCount; i++)
            if( !(this.transform.GetChild(i).name == "DialogueWindow" ||
                this.transform.GetChild(i).name == "MainMenu" ||
                this.transform.GetChild(i).name == "PauseMenu" ||
                this.transform.GetChild(i).name == "SettingsWindow" ||
                this.transform.GetChild(i).name == "ItemWindow"))
                    if(this.transform.GetChild(i).TryGetComponent<WindowController>(out WindowController win)) win.Show();
        Inventory.InventoryManager.Instance.UpdateInventory();

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
            if(this.transform.GetChild(i).name == "MainMenu" ||
                this.transform.GetChild(i).name == "PauseMenu")
                this.transform.GetChild(i).GetComponent<Image>().color = new Color(_rgb.r, _rgb.g, _rgb.b, 1f);
            else
                this.transform.GetChild(i).GetComponent<Image>().color = _rgb;
        this.transform.Find("MenuBtn").GetComponent<Image>().color = Color.white;
    }

    public void CreateDamageNumber(Vector3 _position, uint _damage) {
        GameObject gameObject = Instantiate(
            DamageCanvas.gameObject,
            _position,
            Quaternion.Euler(Vector3.zero)
        );
        gameObject.GetComponent<DamageNumber>().DamagePoints = _damage;
        Destroy(gameObject, 0.5f);
    }

    public void StartDialogue(Dialogue.DialogueController _dialogue) {
        DialogueWindow.GetComponent<WindowController>().Show();
        DialogueWindow.GetComponent<Dialogue.DialogueManager>().CurrentDialogue = _dialogue;
    }

    public void ShowPause() {
        this.transform.Find("PauseMenu").GetComponent<WindowController>().Show();
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void GoToMenu() {
        StatWindow.gameObject.SetActive(false);
        InventoryWindow.gameObject.SetActive(false);
        ItemWindow.gameObject.SetActive(false);
        DialogueWindow.gameObject.SetActive(false);
        SettingsWindow.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);

        MainMenu.gameObject.SetActive(true);

        MainMenu.transform.Find("StartBtn").gameObject.SetActive(System.IO.File.Exists(Application.persistentDataPath + "/save.dat"));
    }

    public void GoToGame() {
        StatWindow.gameObject.SetActive(true);
        InventoryWindow.gameObject.SetActive(true);
        ItemWindow.gameObject.SetActive(false);
        DialogueWindow.gameObject.SetActive(false);
        SettingsWindow.gameObject.SetActive(false);

        OnUIClick = false;

        PauseMenu.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
    }
    public void ShowGameOver(bool _isGood) {
        StatWindow.gameObject.SetActive(false);
        InventoryWindow.gameObject.SetActive(false);
        ItemWindow.gameObject.SetActive(false);
        DialogueWindow.gameObject.SetActive(false);
        SettingsWindow.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);

        GameManager.Instance.Unspawn();

        GameOver.transform.Find("GameEnd").GetComponent<Text>().text = (_isGood ? "You Won!!" : "You Lost!!");
        GameOver.transform.gameObject.SetActive(true);
    }

    public void ShowCredits() {
        StatWindow.gameObject.SetActive(false);
        InventoryWindow.gameObject.SetActive(false);
        ItemWindow.gameObject.SetActive(false);
        DialogueWindow.gameObject.SetActive(false);
        SettingsWindow.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);

        Credits.gameObject.SetActive(true);
    }
}
