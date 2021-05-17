using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemStats : BaseStats {
        public uint Id;
        public string Name;

        public int MaxStackSize;
        public int CurrentAmount;
        public ItemType Type;

        public void OpenItemUI() => UIManager.Instance.ItemUI(this);
        public void Reset(ItemStats _item) {
            this.Id = _item.Id;
            this.Name = _item.Name;
            this.MaxStackSize = _item.MaxStackSize;
            this.CurrentAmount = _item.CurrentAmount;
            this.Type = _item.Type;

            base.Reset(_item);
        }

        public enum ItemType {
            Weapon,
            Consumable,
            Miscelaneous
        }
    }
}