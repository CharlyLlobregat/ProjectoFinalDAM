using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour {
    public static ItemManager Instance {get; private set;}
    private void Awake() => Instance = this;

    public List<Stats.ItemStats> Items;

    public Stats.ItemStats GetItem(Stats.ItemStats _item) =>
        Items
        .First(x => x.Id == _item?.Id);
    public void UseItem(Stats.ItemStats _item) =>
        Items
        .First(x => x.Id == _item?.Id)
        .GetComponent<InteractionController>()
        .OnUse?
        .Invoke();
}