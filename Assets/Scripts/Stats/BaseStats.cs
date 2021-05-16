using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    public class BaseStats : MonoBehaviour {
        #region LEVEL
        public uint Level {
            get => this.lvl;
            set {
                this.lvl = value;

                if(OnLevelChange != null)   OnLevelChange.Invoke(this.lvl);
            }
        }
        [SerializeField] private uint lvl = 1;
        public OnLevelChange OnLevelChange;
        #endregion

        public uint Strength;
        public uint Defense;
        public uint Speed;

        public void Reset(BaseStats _base) {
            this.lvl = _base.lvl;

            this.Strength = _base.Strength;
            this.Defense = _base.Defense;
            this.Speed = _base.Speed;
        }
    }

    #region EVENTS

    [System.Serializable] public class OnLevelChange : UnityEngine.Events.UnityEvent<uint> { }
    [System.Serializable] public class OnExpChange : UnityEngine.Events.UnityEvent<uint, uint> { }
    [System.Serializable] public class OnStatPointsChange : UnityEngine.Events.UnityEvent<uint>{ }
    [System.Serializable] public class OnHealthChange : UnityEngine.Events.UnityEvent<uint, uint> { }
    [System.Serializable] public class OnDead : UnityEngine.Events.UnityEvent { }
    #endregion
}

