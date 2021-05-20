using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [System.Serializable]
    public class DialogueAction : MonoBehaviour {
        public string Name;
        public Sprite Option;
        public UnityEngine.Events.UnityEvent Action;
    }
}