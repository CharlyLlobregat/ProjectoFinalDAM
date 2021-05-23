using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [System.Serializable]
    public class DialogueAction : MonoBehaviour {
        public string Name;
        public Sprite Option;
        public UnityEngine.UI.Button.ButtonClickedEvent Action;

        public uint Uses;
        private uint currentUses = 0;

        private void Awake() {
            this.currentUses = 0;
            this.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = Name;
            this.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Option;
            this.transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                if(Uses > currentUses++)
                    Action?.Invoke();
            });
        }

        public void GivePlayerItem(Stats.ItemStats _item) {
            if(Managers.EntityManager.Instance.GetCurrentEntity("Player", out Stats.EntityStats _entity)) {
                _entity.GetComponent<Inventory.Inventory>().AddItem(_item);
            }
        }
    }
}