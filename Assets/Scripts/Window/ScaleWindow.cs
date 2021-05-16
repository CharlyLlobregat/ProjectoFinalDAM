using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleWindow : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IDragHandler, IDropHandler {
    public CanvasGroup canvasGroup;

    public WindowController Window;
    private float scaleFactor;

    private Vector2 min;
    private Vector2 max;

    private void Start() {
        this.scaleFactor = UIManager.Instance.gameObject.GetComponent<Canvas>().scaleFactor;
        this.min = this.transform.parent.GetComponent<WindowController>().Min;
        this.max = this.transform.parent.GetComponent<WindowController>().Max;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData) {
        RectTransform transform = Window.GetComponent<RectTransform>();
        this.GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y, 0);

        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (transform.sizeDelta.x + eventData.delta.x / this.scaleFactor));
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (transform.sizeDelta.y - eventData.delta.y / this.scaleFactor));

        if(this.min.y > transform.sizeDelta.y)
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.min.y);
        if(this.min.x > transform.sizeDelta.x)
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.min.x);

        if(this.max.x < transform.sizeDelta.x)
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.max.x);
        if (this.max.y < transform.sizeDelta.y)
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.max.y);

        this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData) {

    }

    public void OnPointerUp(PointerEventData eventData) { }
    public void OnDrop(PointerEventData eventData) { }
}