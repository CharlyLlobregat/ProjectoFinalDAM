using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBuilding : MonoBehaviour {
    public GameObject Interior;
    public GameObject Exterior;

    public Transform InteriorPlace;

    private void Start() {
        GameObject.Instantiate(Exterior, this.transform.position, this.transform.rotation);
        GameObject.Instantiate(Interior, InteriorPlace.position, InteriorPlace.rotation);
    }
}