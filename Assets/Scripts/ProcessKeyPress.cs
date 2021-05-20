using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessKeyPress : MonoBehaviour {

    public SettingsManager.KeyBind BindTo;
    private bool recording;
    public void SetRecording(bool _value) => recording = _value;

    private void Awake() {
        KeyCode key = KeyCode.None;
        switch (BindTo) {
            case SettingsManager.KeyBind.Activate:
                key = SettingsManager.Instance.Activate;
                break;
            case SettingsManager.KeyBind.Attack:
                key = SettingsManager.Instance.Attack;
                break;
            case SettingsManager.KeyBind.Talk:
                key = SettingsManager.Instance.Talk;
                break;
            case SettingsManager.KeyBind.Down:
                key = SettingsManager.Instance.Down;
                break;
            case SettingsManager.KeyBind.Left:
                key = SettingsManager.Instance.Left;
                break;
            case SettingsManager.KeyBind.Pick:
                key = SettingsManager.Instance.Pick;
                break;
            case SettingsManager.KeyBind.Right:
                key = SettingsManager.Instance.Right;
                break;
            case SettingsManager.KeyBind.Up:
                key = SettingsManager.Instance.Up;
                break;
        }

        string keyName = System.Enum.GetName(typeof(KeyCode), key);
        if (key == KeyCode.Mouse0) keyName = "Mouse Left";
        else if (key == KeyCode.Mouse1) keyName = "Mouse Right";
        this.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = keyName;
    }
    private void Update() {
        if (this.recording) {
            this.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "";

            foreach(KeyCode key in (KeyCode[]) System.Enum.GetValues(typeof(KeyCode)))
                if(Input.GetKeyDown(key)) {
                    switch (BindTo) {
                        case SettingsManager.KeyBind.Activate:
                            SettingsManager.Instance.Activate = key;
                            break;
                        case SettingsManager.KeyBind.Attack:
                            SettingsManager.Instance.Attack = key;
                            break;
                        case SettingsManager.KeyBind.Talk:
                            SettingsManager.Instance.Talk = key;
                            break;
                        case SettingsManager.KeyBind.Down:
                            SettingsManager.Instance.Down = key;
                            break;
                        case SettingsManager.KeyBind.Left:
                            SettingsManager.Instance.Left = key;
                            break;
                        case SettingsManager.KeyBind.Pick:
                            SettingsManager.Instance.Pick = key;
                            break;
                        case SettingsManager.KeyBind.Right:
                            SettingsManager.Instance.Right = key;
                            break;
                        case SettingsManager.KeyBind.Up:
                            SettingsManager.Instance.Up = key;
                            break;
                    }
                    this.recording = false;

                    string keyName = System.Enum.GetName(typeof(KeyCode), key);
                    if(key == KeyCode.Mouse0)       keyName = "Mouse Left";
                    else if(key == KeyCode.Mouse1)  keyName = "Mouse Right";

                    this.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = keyName;
                }
        }
    }
}
