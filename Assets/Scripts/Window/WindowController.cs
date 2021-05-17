using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DragDrop))]
public class WindowController : MonoBehaviour {
    public Button CloseBtn;
    public Button HideBtn;

    public Vector2 Min;
    public Vector2 Max;

    public bool closed = false;
    private bool hide = false;
    private Vector3 startPosition;

    private void Start() {
        CloseBtn.onClick.AddListener(this.OnClose);
        HideBtn.onClick.AddListener(this.OnHide);

        this.startPosition = this.transform.position;
    }

    public void Show() {
        this.gameObject.SetActive(true);

        this.closed = false;
        this.transform.position = this.startPosition;
    }

    public void OnHide() {
        this.hide = true;
        this.gameObject.SetActive(!this.hide);
    }

    public void OnClose() {
        this.closed = !this.closed;

        this.CloseBtn.transform.rotation = new Quaternion(0, 0, (this.closed ? 0 : 180), 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            this.GetComponent<RectTransform>().sizeDelta.x,
            this.closed ? 30 : Min.y
        );

        this.transform.Find("Content").gameObject.SetActive(!this.closed);
    }
}
