using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMap : MonoBehaviour {
    public List<GameObject> Maps;

    public void ActivateMap(uint _map) {
        Maps.ForEach(x => x.SetActive(false));
        Maps[(int) _map].SetActive(true);
    }
}