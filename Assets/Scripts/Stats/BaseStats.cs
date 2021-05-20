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

                OnLevelChange?.Invoke(this.lvl);
            }
        }
        [Header("BaseStats")]
        [SerializeField] private uint lvl = 1;
        #endregion

        public uint Strength;
        public uint Defense;
        public uint Speed;
        public uint AttackSpeed;

        public string Name;

        public OnLevelChange OnLevelChange;
        public void ResetValues(BaseStats _base) {
            this.lvl = _base.lvl;

            this.Strength = _base.Strength;
            this.Defense = _base.Defense;
            this.Speed = _base.Speed;
            this.AttackSpeed = _base.AttackSpeed;
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

