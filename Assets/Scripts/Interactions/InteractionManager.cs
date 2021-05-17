using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Stats.EntityStats))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inventory))]
public class InteractionManager : MonoBehaviour {
    public List<InteractionController> Interactables;

    private Stats.EntityStats stats;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;

    public Canvas DamageCanvas;

    private void Start() {
        this.stats = GetComponent<Stats.EntityStats>();
        if(!TryGetComponent<SpriteRenderer>(out this.spriteRenderer))
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.inventory = GetComponent<Inventory>();
    }

    public void TakeDamage(uint _damage) {
        uint itemDefense = 0;
        foreach(Stats.ItemStats i in this.inventory.EquipedItems)
            itemDefense += i.Defense;
        uint totalDamage = (uint) Mathf.Max(_damage - itemDefense - this.stats.Defense, 0);

        this.stats.ReduceHealth(totalDamage);

        Instantiate(
            DamageCanvas.gameObject,
            this.transform.position,
            Quaternion.Euler(Vector3.zero)
        ).GetComponent<DamageNumber>().DamagePoints = totalDamage;
    }
    public uint Damage() {
        uint itemDamage = 0;
        foreach(Stats.ItemStats i in this.inventory.EquipedItems)
            itemDamage += i.Strength;

        return this.stats.Strength + itemDamage;
    }

    public void Activate() {
        InteractionController objectToInteract = GetNearest(this.Interactables?.Where(x => x.CanActivate));
        if (objectToInteract == null) return;

        objectToInteract.ActivateAction.Invoke();
    }

    public void Use(Stats.ItemStats _item) {

    }
    public void Pick() {
        InteractionController objectToInteract = GetNearest(this.Interactables?.Where(x => x.CanPick));
        if (objectToInteract == null) return;

        this.GetComponent<Inventory>().AddItem(objectToInteract.GetComponent<Stats.ItemStats>());
        Destroy(objectToInteract.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _other) {
        InteractionController temp = null;
        if (_other.TryGetComponent<InteractionController>(out temp)) {
            this.Interactables.Add(temp);
            Debug.Log(_other.gameObject.name + " Entered");
        }
    }
    private void OnTriggerExit2D(Collider2D _other) {
        InteractionController temp = null;
        if (_other.TryGetComponent<InteractionController>(out temp)) {
            this.Interactables.Remove(temp);
            Debug.Log(_other.gameObject.name + " Left");
        }
    }


    private InteractionController GetNearest(IEnumerable<InteractionController> _from) {
        try {
            return _from?.Where(x => {
                Vector2 direction = this.transform.gameObject.GetComponent<PlayerController>().LastMovement;
                Vector2 objectPosition = (this.transform.position - x.InteractionPoint.position).normalized;

                if (Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.y)) {
                    if ((direction.x > 0 && objectPosition.x < 0) || (direction.x < 0 && direction.x > 0)) return true;
                } else {
                    if ((direction.y > 0 && objectPosition.y < 0) || (direction.y < 0 && direction.y > 0)) return true;
                }

                return false;
            })?.OrderBy(x => {
                return (this.transform.position - x.InteractionPoint.position).magnitude;
            })?.First();
        } catch { }

        return null;
    }

    public void Kill() {
        GameObject.Find("Player").GetComponent<Stats.EntityStats>().AddExp(this.stats.EXP);
        this.inventory.Items.ForEach(x => {
            var item = Instantiate(
                ItemManager.Instance.Items.Find(y => y.Id == x.Id).gameObject,
                this.transform.position + new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    0
                ),
                this.transform.rotation
            );
            item.GetComponent<InteractionController>().CanPick = true;
        });
        this.gameObject.SetActive(false);
    }
}