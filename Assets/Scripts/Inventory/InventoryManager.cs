using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryManager : MonoBehaviour {

    public ToggleGroup InvSelect;
    public ToggleGroup EquipedSelect;

    public Button Add;
    public Button Remove;
    public Button Use;

    public GameObject ItemInv;

    private Inventory inv;

    private void Start() {
        this.inv = GameObject.Find("Player").GetComponent<Inventory>();
        this.Fill();
    }

    public void Equipe() {
        this.inv.Equipe(InvSelect.ActiveToggles().Where(x => x.isOn).First().gameObject.GetComponentInChildren<Stats.ItemStats>());
        Fill();

    }
    public void UnEquipe() {
        this.inv.Unequipe(EquipedSelect.ActiveToggles().Where(x => x.isOn).First().gameObject.GetComponentInChildren<Stats.ItemStats>());
        Fill();
    }

    private void Fill() {
        InvSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));
        EquipedSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));

        Debug.Log("Filling");
        this.inv.EquipedItems.ForEach(x => {
            GameObject tempItem = Instantiate(
                ItemInv,
                EquipedSelect.transform
            );
            tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.GetComponent<SpriteRenderer>().sprite;
            tempItem.GetComponentInChildren<Text>().text = "" + x.CurrentAmount;
            tempItem.GetComponent<Toggle>().group = EquipedSelect;
            tempItem.GetComponent<Toggle>().isOn = false;
            tempItem.GetComponentInChildren<Stats.ItemStats>().Reset(x.GetComponent<Stats.ItemStats>());
            tempItem.GetComponentInChildren<SpriteRenderer>().sprite = x.GetComponent<SpriteRenderer>().sprite;
        });

        this.inv.Items.ForEach(x => {
            GameObject tempItem = Instantiate(
                ItemInv,
                InvSelect.transform
            );
            tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.GetComponent<SpriteRenderer>().sprite;
            tempItem.GetComponentInChildren<Text>().text = "" + x.CurrentAmount;
            tempItem.GetComponent<Toggle>().group = InvSelect;
            tempItem.GetComponent<Toggle>().isOn = false;
            tempItem.GetComponentInChildren<Stats.ItemStats>().Reset(x.GetComponent<Stats.ItemStats>());
            tempItem.GetComponentInChildren<SpriteRenderer>().sprite = x.GetComponent<SpriteRenderer>().sprite;
        });
    }
}