using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Stats.EntityStats))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inventory))]
public class InteractionManager2 : MonoBehaviour {
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
        foreach((Stats.ItemStats i, _) in this.inventory.Equiped)
            itemDefense += i.Defense;
        uint totalDamage = (uint) Mathf.Max(_damage - itemDefense - this.stats.Defense, 0);

        this.stats.ReduceHealth(totalDamage);

        if( this.gameObject.CompareTag("Player") &&
            SettingsManager.Instance.ShowPlayerDamageNumbers &&
            SettingsManager.Instance.ShowDamageNumbers)
                Instantiate(
                    DamageCanvas.gameObject,
                    this.transform.position,
                    Quaternion.Euler(Vector3.zero)
                ).GetComponent<DamageNumber>().DamagePoints = totalDamage;
        else if(    SettingsManager.Instance.ShowEnemyDamageNumbers &&
                    SettingsManager.Instance.ShowDamageNumbers)
                        Instantiate(
                            DamageCanvas.gameObject,
                            this.transform.position,
                            Quaternion.Euler(Vector3.zero)
                        ).GetComponent<DamageNumber>().DamagePoints = totalDamage;
    }
    public uint Damage() {
        uint itemDamage = 0;
        foreach((Stats.ItemStats i, _) in this.inventory.Equiped)
            itemDamage += i.Strength;

        return this.stats.Strength + itemDamage;
    }

    public void Activate() {
        InteractionController objectToInteract = GetNearest(this.Interactables?.Where(x => x.CanActivate));
        if (objectToInteract == null) return;

        objectToInteract.OnActivate?.Invoke();
    }

    public void Use(Stats.ItemStats _item) {
        // TODO
    }

    public void Pick() {
        InteractionController objectToInteract = GetNearest(this.Interactables?.Where(x => x.CanPick));
        if (objectToInteract == null) return;

        this.GetComponent<Inventory>().AddItem(objectToInteract.GetComponent<Stats.ItemStats>());
        Destroy(objectToInteract.gameObject);
    }
    public bool CanAttack() {
        return GetNearest(this.Interactables?.Where(x => x.CanAttack)) != null;
    }

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
            this.Interactables.Add(temp);
    }
    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
            this.Interactables.Remove(temp);
    }

    private InteractionController GetNearest(IEnumerable<InteractionController> _from) {
        try {
            return _from?.Where(x => {
                Vector2 direction = this.transform.gameObject.GetComponent<PlayerController2>().LastMovement;
                Vector2 objectPosition = (this.transform.position - x.InteractionPoint.position).normalized;

                if (Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.y)) {
                    if ((direction.x > 0 && objectPosition.x < 0) || (direction.x < 0 && objectPosition.x > 0)) return true;
                } else {
                    if ((direction.y > 0 && objectPosition.y < 0) || (direction.y < 0 && objectPosition.y > 0)) return true;
                }

                return false;
            })?.OrderBy(x => (this.transform.position - x.InteractionPoint.position).magnitude )
            ?.First();
        } catch { }

        return null;
    }

    public void Kill() {
        if(this.gameObject.activeSelf) {
            GameObject.Find("Player").GetComponent<Stats.EntityStats>().AddExp(this.stats.EXP);
            this.inventory.Items.ForEach(x => {
                var item = Instantiate(
                    ItemManager.Instance.Items.Find(y => y.Id == x.Item.Id).gameObject,
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
}
/**
 * Determina contra que se va a realizar una acción y
 * le pasa esta información al evento del InteractionController para que la realize correctamente.
 */
[RequireComponent(typeof(BaseController))]
[RequireComponent(typeof(InteractionController))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Stats.EntityStats))]
public class InteractionManager : MonoBehaviour {
    public List<InteractionController> Interactables;

    private Stats.EntityStats stats;
    private Inventory inv;

    private void Awake() {
        this.stats = GetComponent<Stats.EntityStats>();
        this.inv = GetComponent<Inventory>();
    }

    public void Attack() {
        GetNearest(this.Interactables?.Where(x => x.CanBeAttacked))?.OnAttacked?.Invoke(this.Damage());
    }
    public void TakeDamage(uint _damage) {
        this.stats.ReduceHealth((uint) Mathf.Max(0, _damage - Defense()));
    }
    public uint Damage() {
        uint itemDamage = 0;
        foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
            itemDamage += i.Strength;

        return this.stats.Strength + itemDamage;
    }
    public uint Defense() {
        uint itemDefense = 0;
        foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
            itemDefense += i.Defense;

        return this.stats.Defense + itemDefense;
    }
    public void Use() {
        GetNearest(this.Interactables.Where(x => x.CanActivate))?.OnActivate?.Invoke();
    }
    public void Talk() {
        // GetNearest(this.Interactables.Where(x => x.CanTalk))?.TalkAction.Invoke();
    }
    public void Place(Stats.ItemStats _item) {
        try{
            var item = Instantiate(
                ItemManager.Instance.GetItem(_item)?.gameObject,
                this.transform.position * this.GetComponent<BaseController>().LastMovement * 0.5f,
                this.transform.rotation
            );
        } catch {
            Debug.Log("Could not be places: " + _item);
        }
    }
    public void Kill() {
        if (this.gameObject.activeSelf) {
            GameObject.Find("Player").GetComponent<Stats.EntityStats>().AddExp(this.stats.EXP);
            this.inv.Items.ForEach(x => {
                var item = Instantiate(
                    ItemManager.Instance.Items.Find(y => y.Id == x.Item.Id).gameObject,
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

    public Stats.ItemStats Pick() => GetNearest(this.Interactables.Where(x => x.CanBePicked))?.GetComponent<Stats.ItemStats>();
    public Stats.ItemStats UseItem() => GetNearest(this.Interactables.Where(x => x.CanBeUsed))?.GetComponent<Stats.ItemStats>();

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
            this.Interactables.Add(temp);
    }
    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
            this.Interactables.Remove(temp);
    }
    private InteractionController GetNearest(IEnumerable<InteractionController> _from) {
        try {
            return _from?.Where(x => {
                Vector2 direction = this.transform.gameObject.GetComponent<PlayerController2>().LastMovement;
                Vector2 objectPosition = (this.transform.position - x.InteractionPoint.position).normalized;

                if (Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.y)) {
                    if ((direction.x > 0 && objectPosition.x < 0) || (direction.x < 0 && objectPosition.x > 0)) return true;
                } else {
                    if ((direction.y > 0 && objectPosition.y < 0) || (direction.y < 0 && objectPosition.y > 0)) return true;
                }

                return false;
            })?.OrderBy(x => (this.transform.position - x.InteractionPoint.position).magnitude)
            ?.First();
        } catch { }

        return null;
    }
}