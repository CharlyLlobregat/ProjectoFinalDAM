using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Teleport {
    public class TeleportPoint : MonoBehaviour {
        [Tooltip("La posición a la que teleportar.")]
        public Transform TeleportOutput;

        [Tooltip("El area que teleporta.")]
        public Collider2D TeleportInput;

        [Tooltip("Acción a ejecutar cuando se teleporta algo.")]
        public UnityEvent OnTeleport;

        public bool ShouldTeleport = true;
        public bool AllowNotPlayers = true;

        private void Start() {
            if(TeleportOutput == null)  this.transform.Find("TeleportOutput");
            if(TeleportInput == null)   this.transform.Find("TeleportInput");

            if(TeleportInput == null || TeleportOutput == null)
                throw new System.NullReferenceException();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (ShouldTeleport)
                if(!AllowNotPlayers && other.CompareTag("Player"))       Move(other.gameObject, TeleportOutput);
                else if(AllowNotPlayers && (other.CompareTag("NPC") || other.CompareTag("Player")))   Move(other.gameObject, TeleportOutput);
        }

        private void Move(GameObject _gameObject, Transform _position) {
            _gameObject.transform.position = _position.position;

            if(_gameObject.CompareTag("Player"))
                Camera.main.transform.position = new Vector3(
                    _position.position.x,
                    _position.position.y,
                    Camera.main.transform.position.z
                );
        }
    }
}

