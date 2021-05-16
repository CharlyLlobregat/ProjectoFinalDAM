using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Teleport {
    public class TeleportPoint2Way : MonoBehaviour {
        [Tooltip("El punto de teleporte 1.")]
        public TeleportPoint InOut1;

        [Tooltip("El punto de teleporte 2.")]
        public TeleportPoint InOut2;

        [Tooltip("Acción a ejecutar cuando se teleporta algo.")]
        public UnityEvent OnTeleport;
        [Tooltip("Acción a ejecutar cuando algo ha sido teleportado.")]
        public UnityEvent OnTeleported;

        public bool ShouldTeleport = true;
        public bool AllowNotPlayers = true;

        private void Start() {
            if (InOut1 == null) this.transform.Find("TeleportPoint1");
            if (InOut2 == null) this.transform.Find("TeleportPoint2");


            if (InOut1 == null || InOut2 == null)
                throw new System.NullReferenceException();
        }
    }
}