using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Controller {
    public class PlayerController : EntityBaseController {
        public static PlayerController Instance {get; private set;}
        protected override void Behaviour() {
            base.Behaviour();

            if (!UIManager.Instance.OnUIClick && this.interact.CanAttack && Input.GetKeyDown(SettingsManager.Instance.Attack))   this.Attack();
            if (!UIManager.Instance.OnUIClick && this.interact.CanTalk && Input.GetKeyDown(SettingsManager.Instance.Talk))       this.Talk();
            if (!UIManager.Instance.OnUIClick && this.interact.CanPick && Input.GetKeyDown(SettingsManager.Instance.Pick))       this.Pick();
            if(Input.GetKeyDown(KeyCode.Escape))                                                UIManager.Instance.ShowPause();

            if (this.interact.CanMove) {
                Vector2 inputDir = Vector2.zero;
                inputDir.x = (Input.GetKey(SettingsManager.Instance.Left) ? -1f : 0f) + (Input.GetKey(SettingsManager.Instance.Right) ? 1f : 0f);
                inputDir.y = (Input.GetKey(SettingsManager.Instance.Down) ? -1f : 0f) + (Input.GetKey(SettingsManager.Instance.Up) ? 1f : 0f);

                this.Move(inputDir);
            }else   this.rigidBody.velocity = Vector2.zero;
        }

        protected override void OnAwake() {
            base.OnAwake();

            Instance = this;
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().Target = this.gameObject;
        }
    }
}
