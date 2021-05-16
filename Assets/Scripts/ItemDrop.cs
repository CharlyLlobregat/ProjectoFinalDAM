using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrop : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null)
            eventData.pointerDrag.transform.position = this.transform.position;
    }
}