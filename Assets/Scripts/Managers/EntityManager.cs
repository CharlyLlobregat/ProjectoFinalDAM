using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Managers {
    public class EntityManager : MonoBehaviour {
        public List<Stats.EntityStats> Entities;

        private void Awake() {
            Entities = new List<Stats.EntityStats>();
        }

        public Stats.EntityStats GetEntity(string _name) => Entities.First(x => x.Name == _name);
    }
}