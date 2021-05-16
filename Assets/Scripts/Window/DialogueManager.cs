using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue {
    public class DialogueManager : MonoBehaviour {
        public DialogueManager Instance {get; private set;}
        private void Start() {
            Instance = this;
            NextDialogue.onClick.AddListener(() => { CurrentDialogue.currentSentence++; });
        }

        public Dialogue CurrentDialogue;
        public Button NextDialogue;
        public GameObject Action;

        public float DeltaLetter;
        private float DeltaLetterCounter;
    }

    [System.Serializable]
    public class Dialogue {
        public List<string> Sentences;
        public uint currentSentence;

        public List<uint> SentenceWithAction;
        public List<DialogueAction> ActionOnSentence;
    }

    [System.Serializable]
    public class DialogueAction {
        public string Name;
        public Sprite Option;
        public UnityEngine.Events.UnityEvent Action;
    }
}
