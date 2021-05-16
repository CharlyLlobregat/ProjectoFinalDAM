using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour {
    public List<Stats.ItemStats> Items;

    public List<Stats.ItemStats> EquipedItems;

    private void Start() {
        Items.Add(ItemManager.Instance.Items.First());
        Items.Add(ItemManager.Instance.Items[1]);
    }

    public void DecreaseAmount(Stats.ItemStats _item) {
        Stats.ItemStats temp = GetItem(_item) ?? GetEquiped(_item);
        if (temp != null && temp.CurrentAmount - 1 > 0) temp.CurrentAmount--;
        else                                            RemoveItem(_item);
    }

    public void IncreaseAmount(Stats.ItemStats _item) {
        Stats.ItemStats temp = GetItem(_item) ?? GetEquiped(_item);
        if(temp != null)    temp.CurrentAmount++;
        else                AddItem(_item);
    }

    public void AddItem(Stats.ItemStats _item) {
        if(Items.Exists(x => x.Id == _item.Id))    IncreaseAmount(_item);
        else                                                Items.Add(_item);
    }
    public void RemoveItem(Stats.ItemStats _item) {
        Items.Remove(GetItem(_item) ?? GetEquiped(_item));
    }

    public Stats.ItemStats GetItem(Stats.ItemStats _item) {
        return Items.Find(x => x.Id == _item.Id);
    }

    public Stats.ItemStats GetEquiped(Stats.ItemStats _item) {
        return EquipedItems.Find(x => x.Id == _item.Id);
    }

    public void Equipe(Stats.ItemStats _item) {
        EquipedItems.Add(GetItem(_item));
        Items.Remove(GetItem(_item));
    }

    public void Unequipe(Stats.ItemStats _item) {
        Items.Add(GetEquiped(_item));
        EquipedItems.Remove(GetEquiped(_item));
    }
}