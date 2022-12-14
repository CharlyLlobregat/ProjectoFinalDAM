using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialogue {
    public class DialogueManager : MonoBehaviour {
        public static DialogueManager Instance {get; private set; }
        private void Awake() => Instance = this;
        private void Start() {
            NextDialogue.onClick.AddListener(() => {
                if(this.currentDialogue.Sentences.Count > this.currentDialogue.currentSentence + 1){
                    this.currentDialogue.currentSentence++;

                    for (int i = 1; i < Actions.transform.childCount; i++)
                        Destroy(Actions.transform.GetChild(i).gameObject);
                    this.currentDialogue.ActionsOnSentence.ForEach(x => {
                        for (int i = 0; i < x; i++)
                            Instantiate(
                                this.currentDialogue.Actions[i],
                                Actions.transform
                            );
                    });
                }
            });

            GetComponent<WindowController>().OnCloseWindow?.AddListener(() => Controller.PlayerController.Instance.GetComponent<Interaction.InteractionController>().IsTalking = false);


            if (DeltaLetter == 0)    DeltaLetter = 0.5f;
        }

        private void Update() {
            if(this.currentDialogue != null)
                if(this.deltaLetterCounter < 0){
                    this.deltaLetterCounter = DeltaLetter;

                    StringBuilder builder = new StringBuilder();

                    for(int i = 0; i < currentLetter; i++)
                        builder.Append(CurrentDialogue.Sentences[(int) CurrentDialogue.currentSentence][i]);
                    if(currentLetter < this.currentDialogue.Sentences[(int) currentDialogue.currentSentence].Length) currentLetter++;

                    Sentences.text = builder.ToString();
                }else
                    this.deltaLetterCounter -= Time.deltaTime;
            else
                this.GetComponent<WindowController>().OnHide();
        }

        public DialogueController CurrentDialogue {
            get => currentDialogue;
            set {
                this.currentDialogue = value;
                if (this.currentDialogue != null) this.currentDialogue.currentSentence = 0;
                else return;
                this.currentLetter = 0;
                this.currentDialogue.currentSentence = 0;
                this.deltaLetterCounter = 0;

                for (int i = 1; i < Actions.transform.childCount; i++)
                    Destroy(Actions.transform.GetChild(i).gameObject);
                this.currentDialogue.ActionsOnSentence.ForEach(x => {
                    for (int i = 0; i < x; i++)
                        Instantiate(
                            this.currentDialogue.Actions[i],
                            Actions.transform
                        );
                });
            }
        }

        [SerializeField] private DialogueController currentDialogue;

        public Button NextDialogue;
        public GameObject Action;
        public Text Sentences;
        public GameObject Actions;

        public float DeltaLetter;
        private float deltaLetterCounter;
        private uint currentLetter;

        public void ClearDialogue(){
            CurrentDialogue = null;
        }
    }
}
