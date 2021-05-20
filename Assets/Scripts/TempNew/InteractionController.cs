using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Stats;

namespace Interaction {
    /**
     * Decide que acciones se pueden realizar,
     * guarda el estado de estas accion y
     * determina que realizar cuando se va a ejecutar una acciones o
     * cuando se recibe una acción que realizar.
     */
    public class InteractionController : MonoBehaviour {
        [Header("Variables")]
        public Transform InteractionPoint;

        [Header("Actions that can be made")]
        public bool CanActivate = false;
        public bool CanPlace = false;
        public bool CanPick = false;
        public bool CanAttack = false;
        public bool CanTalk = false;
        public bool CanMove = false;
        public bool CanUse = false;

        [Header("Actions that can be done to")]
        public bool CanBeActivated = false;
        public bool CanBePlaced = false;
        public bool CanBePicked = false;
        public bool CanBeAttacked = false;
        public bool CanBeTalked = false;
        public bool CanBeMoved = false;
        public bool CanBeUsed = false;

        [Header("Actions currently being done")]
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isTalking;
        [SerializeField] private bool isAttacking;
        public bool IsMoving {
            get => this.isMoving;
            set {
                this.isMoving = value;
                OnIsMovingChange?.Invoke(value);
            }
        }
        public bool IsTalking {
            get => this.isTalking;
            set {
                this.isTalking = value;
                OnIsTalkingChange?.Invoke(value);
            }
        }
        public bool IsAttacking {
            get => this.isAttacking;
            set {
                this.isAttacking = value;
                OnIsAttakingChange?.Invoke(value);
            }
        }


        [Header("Actions to execute on value change events")]
        [SerializeField] public IsMovingChangeEvent OnIsMovingChange;
        [SerializeField] public IsTalkingChangeEvent OnIsTalkingChange;
        [SerializeField] public IsAttakingChangeEvent OnIsAttakingChange;

        [Header("Actions to execute on different event made")]
        [SerializeField] public MoveEvent OnMove;
        [SerializeField] public MoveToEvent OnMoveTo;
        [SerializeField] public AttackEvent OnAttack;
        [SerializeField] public PickEvent OnPick;
        [SerializeField] public TalkEvent OnTalk;
        [SerializeField] public UseEvent OnUse;
        [SerializeField] public PlaceEvent OnPlace;

        [Header("Actions to execute on different events")]
        [SerializeField] public AttackedEvent OnAttacked;
        [SerializeField] public PickedEvent OnPicked;
        [SerializeField] public TalkedEvent OnTalked;
        [SerializeField] public UsedEvent OnUsed;
        [SerializeField] public PlacedEvent OnPlaced;

        #region EVENT_DEFINITIONS
        // OnChange Event Classes
        [System.Serializable] public class IsMovingChangeEvent : UnityEvent<bool> { }
        [System.Serializable] public class IsTalkingChangeEvent : UnityEvent<bool> { }
        [System.Serializable] public class IsAttakingChangeEvent : UnityEvent<bool> { }


        // SELF_ACTION
        [System.Serializable] public class MoveEvent : UnityEvent<Vector2> { }
        [System.Serializable] public class MoveToEvent : UnityEvent<Vector2> { }
        [System.Serializable] public class AttackEvent : UnityEvent<uint> { }

        // DATA_ACTION
        [System.Serializable] public class UseEvent : UnityEvent<ItemStats> { }
        [System.Serializable] public class UsedEvent : UnityEvent { }
        [System.Serializable] public class PlaceEvent : UnityEvent<ItemStats, Vector3> { }
        [System.Serializable] public class PlacedEvent : UnityEvent<Vector3> { }




        // OTHER_ACTION
        [System.Serializable] public class AttackedEvent : UnityEvent<EntityStats, uint> { }
        [System.Serializable] public class PickEvent : UnityEvent { }
        [System.Serializable] public class PickedEvent : UnityEvent<EntityStats> { }
        [System.Serializable] public class TalkEvent : UnityEvent { }
        [System.Serializable] public class TalkedEvent : UnityEvent<EntityStats> { }
        #endregion


        private void Awake() => this.InteractionPoint = this.InteractionPoint != null ? this.InteractionPoint : this.transform;
    }
}
