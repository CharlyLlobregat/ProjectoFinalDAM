using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor2 : MonoBehaviour {

    [Tooltip("El sprite que mostrar con la puerta cerrada.")]
    public Sprite DoorOpen;

    [Tooltip("El sprite que mostrar con la puerta abierta.")]
    public Sprite DoorClosed;

    [Tooltip("Si la puerta deberia abrirse.")]
    public bool ShouldOpen = true;

    private bool opened = false;
    private bool canInteract = false;
    private GameObject doorSprite;

    public void Start() {
        this.doorSprite = this.transform.Find("DoorSprite").gameObject;

        if(DoorOpen == null || DoorClosed == null || this.doorSprite == null)  throw new System.NullReferenceException();

        opened = false;
        if(this.doorSprite.GetComponent<SpriteRenderer>().sprite == null)   this.doorSprite.GetComponent<SpriteRenderer>().sprite = DoorClosed;
    }

    public void Open() {
        if (!this.opened) {
            this.doorSprite.GetComponent<SpriteRenderer>().sprite = DoorOpen;
            this.doorSprite.GetComponent<Collider2D>().enabled = false;
            this.opened = true;
        }
    }

    public void Close() {
        if (this.opened) {
            this.doorSprite.GetComponent<SpriteRenderer>().sprite = DoorClosed;
            this.doorSprite.GetComponent<Collider2D>().enabled = true;
            this.opened = false;
        }
    }

    private void Interact() {
        if(this.opened) Close();
        else if(!this.opened) Open();
        Debug.Log("Intereacted");
    }


    private void Update() {
        if(this.canInteract && Input.GetMouseButtonDown(0)) Interact();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(!this.canInteract)    this.canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(this.canInteract)    this.canInteract = false;
        if (this.opened)        StartCoroutine(closeDoor());
    }

    private IEnumerator closeDoor() {
        yield return new WaitForSeconds(1);
        Close();
    }
}