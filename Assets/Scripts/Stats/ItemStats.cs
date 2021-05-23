using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Stats {
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemStats : BaseStats {
        [Header("Item Stats")]
        public uint Id;

        public int MaxStackSize;
        public ItemType Type;
        public WeaponType Weapon;

        public void OpenItemUI() => UIManager.Instance.ItemUI(this);
        public void Reset(ItemStats _item) {
            this.Id = _item.Id;
            this.Name = _item.Name;
            this.MaxStackSize = _item.MaxStackSize;
            this.Type = _item.Type;
            this.Weapon = _item.Weapon;

            base.ResetValues(_item);
        }

        public enum ItemType {
            Weapon,
            Consumable,
            Miscelaneous
        }
        public enum WeaponType {
            None,
            Sword,
            Arc,
            Arrow
        }
    }
}