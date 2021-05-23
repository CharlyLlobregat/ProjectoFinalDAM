using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerClickHandler /*, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler */ {

    /*
    private Vector3 startPosition;
    private Transform startParent;

    public void OnBeginDrag(PointerEventData eventData) {
        this.startPosition = this.transform.position;
        this.startParent = this.transform;

        this.transform.SetParent(UIManager.Instance.transform);
    }

    public void OnDrag(PointerEventData eventData) {
        this.transform.Translate(eventData.delta);
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag);
    }

    public void OnEndDrag(PointerEventData eventData) {

    }
    */

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.clickCount == 2) {
            if(this.transform.parent.parent.parent.name == "InvItems")
                Inventory.InventoryManager.Instance.Equipe(GetComponent<Stats.ItemStats>());
            else if(this.transform.parent.parent.name == "EquipedItems")
                Inventory.InventoryManager.Instance.UnEquipe(GetComponent<Stats.ItemStats>());
        }
    }
}