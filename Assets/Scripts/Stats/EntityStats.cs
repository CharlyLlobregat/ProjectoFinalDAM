using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    public class EntityStats : BaseStats {
        private void Start() {
            OnExpChange?.Invoke(this.exp, ExpPerLvl(Level));
            OnStatPointsChange?.Invoke(this.statPoints);
            OnHealthChange?.Invoke(this.currentHp, this.hp);
            OnLevelChange?.Invoke(this.Level);
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

                OnExpChange?.Invoke(this.exp, ExpPerLvl(Level));
            }
        }
        [Header("Entity Stats")]
        public bool IsPlayer = false;
        [SerializeField] private uint exp;

        public Func<uint, uint> ExpPerLvl = (lvl) => lvl * 100;

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

                OnStatPointsChange?.Invoke(this.statPoints);
            }
        }

        [SerializeField] private uint statPoints;

        public uint StatsPerLvl = 10;


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

                OnHealthChange?.Invoke(this.currentHp, this.hp);
            }
        }

        public void Heal(uint _amount) => CurrentHealth += _amount;
        public void ReduceHealth(uint _amount) {
            if ((int)(this.currentHp - _amount) <= 0) {
                CurrentHealth = 0;
                OnDead?.Invoke();
            } else
                CurrentHealth -= _amount;
        }

        #endregion

        public OnStatPointsChange OnStatPointsChange;
        public OnHealthChange OnHealthChange;
        public OnDead OnDead;
        public OnExpChange OnExpChange;
    }
}