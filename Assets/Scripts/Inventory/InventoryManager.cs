using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Stats;
using Interaction;

namespace Inventory {
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
        }
        private void Start() {
            if(Managers.EntityManager.Instance.GetCurrentEntity("Player", out Stats.EntityStats _entity)) {
                this.inv = _entity.GetComponent<Inventory>();
                this.Fill();
            }
        }

        public void UpdateInventory() => this.Start();


        public void Equipe() {
            Debug.Log(this.inv.gameObject);
            if(InvSelect.ActiveToggles().Any(x => x.isOn)) {
                this.inv.Equipe(
                    ItemManager.Instance.GetItem(InvSelect
                    .ActiveToggles()?
                    .First(x => x.isOn)?
                    .gameObject?
                    .GetComponent<Stats.ItemStats>())
                );

                Fill();
            }
        }
        public void Equipe(ItemStats _item) {
            this.inv.Equipe(ItemManager.Instance.GetItem(_item));
            Fill();
        }

        public void UnEquipe() {
            Debug.Log(this.inv.gameObject);
            if (EquipedSelect.ActiveToggles().Any(x => x.isOn)) {
                if(EquipedSelect
                    .ActiveToggles()
                    .First(x => x.isOn)
                    .gameObject
                    .TryGetComponent<Stats.ItemStats>(out Stats.ItemStats _item))
                        this.inv.Unequipe(ItemManager.Instance.GetItem(_item));

                Fill();
            }
        }
        public void UnEquipe(ItemStats _item) {
            this.inv.Unequipe(ItemManager.Instance.GetItem(_item));
            Fill();
        }

        public void Use() {
            if(InvSelect.ActiveToggles().Any(x => x.isOn)) {
                Controller.PlayerController.Instance
                .GetComponent<Interaction.InteractionController>()?
                .OnUse?
                .Invoke(
                    ItemManager.Instance.GetItem(
                        InvSelect
                        .ActiveToggles()?
                        .First(x => x.isOn)?
                        .gameObject?
                        .GetComponentInChildren<Stats.ItemStats>()
                    )
                );

                Fill();
            }
        }
        public void Drop() {
            if (InvSelect.ActiveToggles().Any(x => x.isOn)) {
                Controller.PlayerController.Instance
                .GetComponent<Interaction.InteractionController>()?
                .OnPlace?
                .Invoke(
                    ItemManager.Instance.GetItem(
                        InvSelect
                        .ActiveToggles()?
                        .First(x => x.isOn)?
                        .gameObject?
                        .GetComponentInChildren<Stats.ItemStats>()
                    ),
                    Controller.PlayerController.Instance.transform.position + new Vector3(
                        Controller.PlayerController.Instance.LastMovement.x * 0.5f,
                        Controller.PlayerController.Instance.LastMovement.y * 0.5f,
                        0f
                    )
                );

                Fill();
            }
        }

        public void Fill() {
            if (Managers.EntityManager.Instance.GetCurrentEntity("Player", out Stats.EntityStats _entity)) {
                this.inv = _entity.GetComponent<Inventory>();
            }

            InvSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));
            EquipedSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => Destroy(x.gameObject));

            //Lets forget there is an error here that sometime happen
            try{
                this.inv.Items?.ForEach(x => {
                    if(this.inv.Equiped.Contains(x) && x.Amount > 1) {
                        GameObject tempItem = Instantiate(
                            ItemInv,
                            InvSelect.transform
                        );
                        tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
                        tempItem.GetComponentInChildren<Text>().text = "" + (x.Amount - 1);
                        tempItem.GetComponent<Toggle>().group = InvSelect;
                        tempItem.GetComponent<Toggle>().isOn = false;
                        tempItem.GetComponent<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
                    }else if (!this.inv.Equiped.Contains(x)) {
                        GameObject tempItem = Instantiate(
                            ItemInv,
                            InvSelect.transform
                        );
                        tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
                        tempItem.GetComponentInChildren<Text>().text = "" + x.Amount;
                        tempItem.GetComponent<Toggle>().group = InvSelect;
                        tempItem.GetComponent<Toggle>().isOn = false;
                        tempItem.GetComponent<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
                    }
                });

                this.inv.Equiped?.ForEach(x => {
                    GameObject tempItem = Instantiate(
                        ItemInv,
                        EquipedSelect.transform
                    );
                    tempItem.transform.Find("Image").GetComponent<Image>().sprite = x.Item.GetComponent<SpriteRenderer>().sprite;
                    tempItem.GetComponentInChildren<Text>().text = "" + (x.Amount > 1 ? 1 : x.Amount);
                    tempItem.GetComponent<Toggle>().group = EquipedSelect;
                    tempItem.GetComponent<Toggle>().isOn = false;
                    tempItem.GetComponent<Stats.ItemStats>().Reset(x.Item.GetComponent<Stats.ItemStats>());
                });
            }catch{ }
        }

        public void UnSelectAll() {
            InvSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => x.isOn = false);
            EquipedSelect.GetComponentsInChildren<Toggle>().ToList().ForEach(x => x.isOn = false);
        }
    }
}