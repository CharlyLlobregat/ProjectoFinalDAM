using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Managers {
    public class EntityManager : MonoBehaviour {
        public static EntityManager Instance {get; private set;}
        public List<Stats.EntityStats> Entities;
        public List<Stats.EntityStats> CurrentEntities;

        private void Awake() {
            Instance = this;
            if(Entities == null) Entities = new List<Stats.EntityStats>();
            CurrentEntities = new List<Stats.EntityStats>();
        }

        public bool GetEntity(string _name, out Stats.EntityStats _entity) {
            try {
                _entity = Entities.First(x => x.Name == _name);
                return true;
            } catch {
                _entity = null;
            }
            return false;
        }
        public bool GetCurrentEntity(string _name, out Stats.EntityStats _entity) {
            try {
                _entity = CurrentEntities.First(x => x.Name == _name);
                return true;
            } catch {
                _entity = null;
            }
            return false;
        }
    }
}