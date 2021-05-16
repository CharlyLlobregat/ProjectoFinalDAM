using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager Instance {get; private set;}
    private void Awake() => Instance = this;

    public List<Stats.ItemStats> Items;
}