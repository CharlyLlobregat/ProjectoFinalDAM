using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Stats;
using System.IO;

namespace Inventory {
    public class Inventory : MonoBehaviour, ISave {
        private void Awake() {
            this.amount = new List<uint>(ItemManager.Instance.Items.Count);
            this.equiped = new List<bool>(ItemManager.Instance.Items.Count);

            ItemManager.Instance.Items.ForEach(_ => {
                this.amount.Add(0);
                this.equiped.Add(false);
            });

            this.BaseItemId.ForEach(x => {
                for(int i = 0; i < ItemManager.Instance.Items.Count; i++)
                    if(ItemManager.Instance.Items[i].Id == x)
                        this.amount[i] = this.amount[i] + 1;
            });
            this.EquipeItemId.ForEach(x => {
                var _item = ItemManager.Instance.Items.First(y => y.Id == x);
                switch (_item.Type) {
                    case ItemStats.ItemType.Weapon:
                        if (Equiped.Exists(z => z.Item.Type == ItemStats.ItemType.Weapon)) return;

                        IsWeaponEquiped?.Invoke(_item.GetComponent<SpriteRenderer>().sprite, true);
                        break;
                    case ItemStats.ItemType.Miscelaneous:
                        break;
                }

                this.equiped[ItemManager.Instance.Items.FindIndex(z => z.Id == _item.Id)] = true;
            });
        }

        public List<(Stats.ItemStats Item, uint Amount)> Items {
            get {
                try{
                    return ItemManager.Instance.Items.Select<Stats.ItemStats, (Stats.ItemStats Item, uint Amount)>((x, pos) =>
                        this.amount[pos] > 0 ? (Item: x, Amount: this.amount[pos]) : (Item: null, Amount: uint.MinValue)
                    )?.Where(x => x.Item != null && x.Amount > 0)?.ToList();
                }catch{
                    return null;
                }
            }
        }
        public List<(Stats.ItemStats Item, uint Amount)> Equiped {
            get {
                return ItemManager.Instance.Items.Select<Stats.ItemStats, (ItemStats Item, uint Amount)>((x, pos) =>
                    this.equiped[pos] ? (Item: x, Amount: this.amount[pos]) : (Item: null, Amount : 0)
                )?.Where(x => x.Item != null && x.Amount > 0)?.ToList();
            }
        }

        public List<bool> equiped;
        public List<uint> amount;

        public List<uint> BaseItemId;
        public List<uint> EquipeItemId;
        public void AddItem(ItemStats _item) {
            if(Items.Exists(x => x.Item.Id == _item.Id))    IncrementAmount(_item);
            else {
                this.amount[ItemManager.Instance.Items.FindIndex(x => x.Id == _item.Id)] = 1;
                OnInventoryChange?.Invoke();
                OnItemAdd?.Invoke(_item, 1);
            }
        }

        public void RemoveItem(ItemStats _item) {
            if(Items.Exists(x => x.Item.Id == _item.Id))    DecrementAmount(_item);
        }

        public void IncrementAmount(ItemStats _item) {
            uint amount = ++this.amount[ItemManager.Instance.Items.FindIndex(x => x.Id == _item.Id)];
            OnItemAdd?.Invoke(_item, amount);
            OnInventoryChange?.Invoke();
        }

        public void DecrementAmount(ItemStats _item) {
            int pos = ItemManager.Instance.Items.FindIndex(x => x.Id == _item.Id);
            if(this.amount[pos] == 0) return;

            uint amount = --this.amount[pos];
            OnItemRemove?.Invoke(_item, amount);
            OnInventoryChange?.Invoke();
        }

        public void Equipe(ItemStats _item) {
            switch (_item.Type) {
                case ItemStats.ItemType.Weapon:
                    if(Equiped.Exists(x => x.Item.Type == ItemStats.ItemType.Weapon)) return;

                    IsWeaponEquiped?.Invoke(_item.GetComponent<SpriteRenderer>().sprite, true);
                    break;
            }

            this.equiped[ItemManager.Instance.Items.FindIndex(x => x.Id == _item.Id)] = true;
            OnItemEquiped?.Invoke(_item);
            OnInventoryChange?.Invoke();
        }

        public void Unequipe(ItemStats _item) {
            switch (_item.Type) {
                case ItemStats.ItemType.Weapon:
                    IsWeaponEquiped?.Invoke(null, false);
                    break;
            }

            this.equiped[ItemManager.Instance.Items.FindIndex(x => x.Id == _item.Id)] = false;
            OnItemUnequiped?.Invoke(_item);
            OnInventoryChange?.Invoke();
        }

        public void UpdateInventory() {
            InventoryManager.Instance.Fill();
        }

        public void OnSave(BinaryWriter _writer) {
            _writer.Write(this.amount.Count);
            this.amount.ForEach(x => _writer.Write(x));
            this.equiped.ForEach(x => _writer.Write(x));
        }

        public void OnLoad(BinaryReader _reader) {
            int amount = _reader.ReadInt32();
            for(int i = 0; i < amount; i++) this.amount[i] = _reader.ReadUInt32();
            for(int i = 0; i < amount; i++) {
                this.equiped[i] = _reader.ReadBoolean();
                if(this.equiped[i] && ItemManager.Instance.Items[i].Type == ItemStats.ItemType.Weapon)
                    this.IsWeaponEquiped?.Invoke(ItemManager.Instance.Items[i].GetComponent<SpriteRenderer>().sprite, true);
            }
        }

        /*
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
        */

        public UnityEngine.Events.UnityEvent OnInventoryChange;
        public ItemAddEvent OnItemAdd;
        public ItemRemoveEvent OnItemRemove;
        public ItemEquipedEvent OnItemEquiped;
        public ItemUnequipedEvent OnItemUnequiped;
        public WeaponEquipedEvent IsWeaponEquiped;

        [System.Serializable] public class ItemAddEvent : UnityEngine.Events.UnityEvent<ItemStats, uint>{}
        [System.Serializable] public class ItemRemoveEvent : UnityEngine.Events.UnityEvent<ItemStats, uint>{}
        [System.Serializable] public class ItemEquipedEvent : UnityEngine.Events.UnityEvent<ItemStats>{ }
        [System.Serializable] public class ItemUnequipedEvent : UnityEngine.Events.UnityEvent<ItemStats>{ }
        [System.Serializable] public class WeaponEquipedEvent : UnityEngine.Events.UnityEvent<Sprite, bool>{ }
    }
}