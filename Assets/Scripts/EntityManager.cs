using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityManager : MonoBehaviour {
    List<Stats.EntityStats> Entities;

    public Stats.EntityStats GetEntity(string _name) => Entities.First(x => x.Name == _name);
}