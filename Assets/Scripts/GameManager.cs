using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SettingsManager))]
[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(EntityManager))]
public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set;}

    private SettingsManager settings;
    private ItemManager items;
    private EntityManager entities;

    private void Awake() {
        Instance = this;
        this.settings = GetComponent<SettingsManager>();
        this.items = GetComponent<ItemManager>();
        this.entities = GetComponent<EntityManager>();
    }

    public void InstantiateArrow(Stats.ItemStats _arrow, uint _damage) {
        GameObject arrow = Instantiate(
            this.entities.GetEntity("Arrow").gameObject,
            Controller.PlayerController.Instance.transform.position,
            Quaternion.Euler(Vector3.zero)
        );
        arrow.GetComponent<SpriteRenderer>().sprite = this.items.GetItem(_arrow).GetComponent<SpriteRenderer>().sprite;
        arrow.GetComponent<Controller.ArrowController>().MoveDirection(Controller.PlayerController.Instance.LastMovement);

        Destroy(arrow, arrow.GetComponent<Stats.EntityStats>().Health);
    }

    public void InstantiateItem(Stats.ItemStats _item, Vector3 _position) {
        GameObject item = Instantiate(
            ItemManager.Instance.GetItem(_item).gameObject,
            _position,
            Quaternion.Euler(Vector3.zero)
        );
        Destroy(item, 10f);
    }
}
