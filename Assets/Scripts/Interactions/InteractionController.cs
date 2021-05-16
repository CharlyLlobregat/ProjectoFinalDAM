using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ActivateEvent : UnityEngine.Events.UnityEvent { }
[System.Serializable] public class PlaceEvent : UnityEngine.Events.UnityEvent<GameObject> { }

public class InteractionController : MonoBehaviour {
    public Transform InteractionPoint;

    public bool CanActivate = false;
    public bool CanPlace = false;
    public bool CanPick = false;
    public bool CanAttack = false;
    public bool CanTalk = false;

    [SerializeField] public ActivateEvent ActivateAction;
    [SerializeField] public PlaceEvent PlaceAction;
}
