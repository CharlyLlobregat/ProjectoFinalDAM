using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance {get; private set;}
    private void Awake() => Instance = this;

    [Header("Interface")]
    public bool ShowDamageNumbers = true;
    public bool ShowPlayerDamageNumbers = false;
    public bool ShowEnemyDamageNumbers = true;
    public bool ShowEnemyHealthBar = false;

    [Header("Interaction")]
    public KeyCode Attack = KeyCode.Mouse0;
    public KeyCode Pick = KeyCode.Mouse0;
    public KeyCode Activate = KeyCode.Mouse0;
    public KeyCode Talk = KeyCode.Mouse0;
    public KeyCode UseItem = KeyCode.Return;

    [Header("Movement")]
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
}
