using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class DialogueController: MonoBehaviour {
        public List<string> Sentences;
        public uint currentSentence;

        public List<uint> ActionsOnSentence;
        public List<DialogueAction> Actions;
    }
}
