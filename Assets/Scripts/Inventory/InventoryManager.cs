using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Stats;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance {get; private set;}

    public ToggleGroup InvSelect;
    public ToggleGroup EquipedSelect;

    public Button AddBtn;
    public Button RemoveBtn;
    public Button UseBtn;

    public GameObject ItemInv;

    private Inventory inv;

    private void Awake() {
        Instance = this;
        this.inv = GameObject.Find("Player").GetComponent<Inventory>();
        this.Fill();
    }

    public void Equipe() {
        if(InvSelect.ActiveToggles().Any(x => x.isOn)) {
            this.inv.Equipe(
                InvSelect
                .ActiveToggles()?
                .First(x => x.isOn)?
                .gameObject?
                .GetComponentInChildren<Stats.ItemStats>()
            );

            Fill();
        }
    }
    public void UnEquipe() {
        if (EquipedSelect.ActiveToggles().Any(x => x.isOn)) {
            this.inv.Unequipe(
                EquipedSelect
                .ActiveToggles()?
                .First(x => x.isOn)?
                .gameObject?
                .GetComponentInChildren<Stats.ItemStats>()
            );

            Fill();
        }
    }
    public void Use() {
        if(InvSelect.ActiveToggles().Any(x => x.isOn)) {
            ItemManager.Instance.GetItem(
                InvSelect
                .ActiveToggles()?
                .First(x => x.isOn)?
                .gameObject?
                .GetComponentInChildren<Stats.ItemStats>()
            ).GetComponent<InteractionController>()?
            .OnUse?
            .Invoke();

            Fill();
        }
    }
    public void Drop() {
        if (InvSelect.ActiveToggles().Any(x => x.isOn)) {
            ItemManager.Instance.GetItem(
                InvSelect
                .ActiveToggles()?
                .First(x => x.isOn)?
                .gameObject?
                .GetComponentInChildren<Stats.ItemStats>()
            ).GetComponent<InteractionController>()?
            .OnPlace?
            .Invoke();

            Fill();
        }
    }

    public void Fill() {
        InvSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));
        EquipedSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));

        this.inv.Items.ForEach(x => {
            if(this.inv.Equiped.Contains(x) && x.Amount > 1) {
                GameObject tempItem = Instantiate(
                    ItemInv,
                    InvSelect.transform
                );
                tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
                tempItem.GetComponentInChildren<Text>().text = "" + (x.Amount - 1);
                tempItem.GetComponent<Toggle>().group = InvSelect;
                tempItem.GetComponent<Toggle>().isOn = false;
                tempItem.GetComponentInChildren<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
                tempItem.GetComponentInChildren<SpriteRenderer>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
            }else if (!this.inv.Equiped.Contains(x)) {
                GameObject tempItem = Instantiate(
                    ItemInv,
                    InvSelect.transform
                );
                tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
                tempItem.GetComponentInChildren<Text>().text = "" + x.Amount;
                tempItem.GetComponent<Toggle>().group = InvSelect;
                tempItem.GetComponent<Toggle>().isOn = false;
                tempItem.GetComponentInChildren<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
                tempItem.GetComponentInChildren<SpriteRenderer>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
            }
        });

        Debug.Log("Filling");
        this.inv.Equiped.ForEach(x => {
            GameObject tempItem = Instantiate(
                ItemInv,
                EquipedSelect.transform
            );
            tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
            tempItem.GetComponentInChildren<Text>().text = "" + (x.Amount > 1 ? 1 : x.Amount);
            tempItem.GetComponent<Toggle>().group = EquipedSelect;
            tempItem.GetComponent<Toggle>().isOn = false;
            tempItem.GetComponentInChildren<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
            tempItem.GetComponentInChildren<SpriteRenderer>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
        });
    }

    public void UnSelectAll() {
        InvSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => x.isOn = false);
        EquipedSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => x.isOn = false);
    }
}