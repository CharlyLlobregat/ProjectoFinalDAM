using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public bool IsTalking{
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


    [Header("Actions to execute on different events")]
    [SerializeField] public OnAttackedEvent OnAttacked;

    [Header("Actions to execute on different event made")]
    [SerializeField] public ActivateEvent OnActivate;
    [SerializeField] public PlaceEvent OnPlace;
    [SerializeField] public TalkEvent OnTalk;
    [SerializeField] public PickEvent OnPick;
    [SerializeField] public MoveEvent OnMove;
    [SerializeField] public AttackEvent OnAttack;
    [SerializeField] public UseEvent OnUse;

    [Header("Actions to execute on value change events")]
    [SerializeField] public IsMovingChangeEvent OnIsMovingChange;
    [SerializeField] public IsTalkingChangeEvent OnIsTalkingChange;
    [SerializeField] public IsAttakingChangeEvent OnIsAttakingChange;

    private void Awake() {
        if (InteractionPoint == null) InteractionPoint = this.transform;
    }

    // OnChange Event Classes
    [System.Serializable] public class IsMovingChangeEvent : UnityEvent<bool> { }
    [System.Serializable] public class IsTalkingChangeEvent : UnityEvent<bool> { }
    [System.Serializable] public class IsAttakingChangeEvent : UnityEvent<bool> { }

    // Action Event Classes
    [System.Serializable] public class ActivateEvent : UnityEvent{ }
    [System.Serializable] public class PlaceEvent : UnityEvent { }
    [System.Serializable] public class AttackEvent : UnityEvent { }
    [System.Serializable] public class PickEvent : UnityEvent { }
    [System.Serializable] public class MoveEvent : UnityEvent { }
    [System.Serializable] public class TalkEvent : UnityEvent { }
    [System.Serializable] public class UseEvent : UnityEvent { }

    [System.Serializable] public class OnAttackedEvent : UnityEvent<uint>{ }

}