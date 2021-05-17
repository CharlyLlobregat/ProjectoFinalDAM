using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Stats;

public class Inventory : MonoBehaviour {
    public WeaponDamage Weapon;

    public List<Stats.ItemStats> Items;
    public List<Stats.ItemStats> EquipedItems;

    /*public List<(Stats.ItemStats, uint)> EquipedItems {
        get {
            return ItemManager.Instance.Items.Select<Stats.ItemStats, (ItemStats, uint)>((x, pos) => {
                return this.equiped[pos] ? (x, uint.MinValue) : (null, 0);
            }).Where(x => x.Item1 != null).ToList();
        }
        set {
            EquipedItems = value;
        }
    }
    */
    private List<bool> equiped;
    private List<uint> amount;

    private void Start() {
        ItemManager.Instance.Items.ForEach(x => {
            Items.Add(x);
        });
    }

    public void DecreaseAmount(Stats.ItemStats _item) {
        Stats.ItemStats temp = GetItem(_item) ?? GetEquiped(_item);
        if (temp != null && temp.CurrentAmount - 1 > 0){
            temp.CurrentAmount--;
            if (OnInventoryChange != null) OnInventoryChange.Invoke();
        } else if(temp != null)                           RemoveItem(_item);
    }

    public void IncreaseAmount(Stats.ItemStats _item) {
        Stats.ItemStats temp = GetItem(_item) ?? GetEquiped(_item);
        if(temp == null)    AddItem(_item);
        else {
            temp.CurrentAmount++;
            if (OnInventoryChange != null) OnInventoryChange.Invoke();
        }
    }

    public void AddItem(Stats.ItemStats _item) {
        if(GetItem(_item) || GetEquiped(_item)) IncreaseAmount(_item);
        else {
            Items.Add(_item);
            if(OnInventoryChange != null)   OnInventoryChange.Invoke();
        }
    }
    public void RemoveItem(Stats.ItemStats _item) {
        Items.Remove(GetItem(_item) ?? GetEquiped(_item));
        if (OnInventoryChange != null) OnInventoryChange.Invoke();
    }

    public Stats.ItemStats GetItem(Stats.ItemStats _item) {
        return Items.Find(x => x.Id == _item.Id);
    }

    public Stats.ItemStats GetEquiped(Stats.ItemStats _item) {
        return EquipedItems.Find(x => x.Id == _item.Id);
    }

    public void Equipe(Stats.ItemStats _item) {
        switch (_item.Type) {
            case ItemStats.ItemType.Weapon:
                    if(EquipedItems.Exists(x => x.Type == ItemStats.ItemType.Weapon)) break;

                    EquipedItems.Add(GetItem(_item));
                    Items.Remove(GetItem(_item));

                    Weapon.transform.Find("Sword").GetComponent<SpriteRenderer>().sprite = _item.GetComponent<SpriteRenderer>().sprite;
                    Weapon.gameObject.SetActive(true);
                break;
            default:
                    EquipedItems.Add(GetItem(_item));
                    Items.Remove(GetItem(_item));
                break;
        }

        if (OnInventoryChange != null) OnInventoryChange.Invoke();
    }

    public void Unequipe(Stats.ItemStats _item) {
        if(_item.Type == ItemStats.ItemType.Weapon)
            Weapon.gameObject.SetActive(false);
        Items.Add(GetEquiped(_item));
        EquipedItems.Remove(GetEquiped(_item));

        if (OnInventoryChange != null) OnInventoryChange.Invoke();
    }

    public UnityEngine.Events.UnityEvent OnInventoryChange;
}