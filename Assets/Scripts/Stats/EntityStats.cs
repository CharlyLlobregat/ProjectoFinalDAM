using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    public class EntityStats : BaseStats {
        private void Start() {
            if (OnExpChange != null) OnExpChange.Invoke(this.exp, ExpPerLvl(Level));
            if (OnStatPointsChange != null) OnStatPointsChange.Invoke(this.statPoints);
            if (OnHealthChange != null) OnHealthChange.Invoke(this.currentHp, this.hp);
            if (OnLevelChange != null) OnLevelChange.Invoke(this.Level);
        }
        #region EXP
        public uint EXP {
            get => this.exp;
            set {
                this.exp = value;
                while (this.exp >= ExpPerLvl(Level)) {
                    this.exp -= ExpPerLvl(Level);

                    this.LevelUp();
                }

                if (OnExpChange != null) OnExpChange.Invoke(this.exp, ExpPerLvl(Level));
            }
        }
        [SerializeField] private uint exp;

        public Func<uint, uint> ExpPerLvl = (lvl) => {
            return lvl * 100;
        };

        public OnExpChange OnExpChange;

        public void AddExp(uint _exp) {
            EXP += _exp;
        }
        public void LevelUp() {
            Level++;
            AddStats(StatsPerLvl);
        }
        #endregion

        #region STATPOINTS
        public uint StatPoints {
            get => this.statPoints;
            set {
                this.statPoints = value;

                if (OnStatPointsChange != null) OnStatPointsChange.Invoke(this.statPoints);
            }
        }

        [SerializeField] private uint statPoints;

        public uint StatsPerLvl = 10;

        public OnStatPointsChange OnStatPointsChange;

        public void AddStats(uint _stats) {
            StatPoints += _stats;
        }
        #endregion

        #region HEALTH
        [SerializeField] private uint hp = 10;
        [SerializeField] private uint currentHp = 10;

        public uint Health {
            get => this.hp;
            set => this.hp = value;
        }
        public uint CurrentHealth {
            get => this.currentHp;
            set {
                if (value > this.hp) this.currentHp = this.hp;
                else this.currentHp = value;

                if (OnHealthChange != null) OnHealthChange.Invoke(this.currentHp, this.hp);
            }
        }

        public void Heal(uint _amount) => CurrentHealth += _amount;
        public void ReduceHealth(uint _amount) {
            if ((int)(this.currentHp - _amount) <= 0) {
                this.currentHp = 0;
                if (OnDead != null) OnDead.Invoke();
            } else
                this.currentHp -= _amount;
        }

        public OnHealthChange OnHealthChange;
        public OnDead OnDead;
        #endregion
    }
}