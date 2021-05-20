using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IDragHandler, IDropHandler {
    public CanvasGroup canvasGroup;
    private Vector3 startPosition;

    public bool Dropped;
    public bool ShouldDrop;
    public bool Parent;
    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
        this.startPosition = eventData.position;
        Dropped = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if(!Parent)     this.transform.Translate(eventData.delta);
        else            this.transform.parent.Translate(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if(!Dropped && ShouldDrop)    this.transform.position = this.startPosition;
    }

    public void OnPointerDown(PointerEventData eventData) {

    }

    public void OnPointerUp(PointerEventData eventData) {}
    public void OnDrop(PointerEventData eventData) {}
}