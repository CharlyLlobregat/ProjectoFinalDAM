using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
namespace Interaction {
    //Maybe Controller
    public class InteractionManager2 : MonoBehaviour, IUse {
        public List<InteractionController> interactables = new List<InteractionController>();

        private void Start() {
            if(!TryGetComponent<Collider2D>(out _)) throw new System.NullReferenceException();
        }

        private void OnTriggerEnter2D(Collider2D _other) {
            InteractionController temp = null;
            if(_other.TryGetComponent<InteractionController>(out temp)) {
                this.interactables.Add(temp);
                Debug.Log(_other.gameObject.name + " Entered");
            }
        }


        private void OnTriggerExit2D(Collider2D _other) {
            InteractionController temp = null;
            if (_other.TryGetComponent<InteractionController>(out temp)) {
                this.interactables.Remove(temp);
                Debug.Log(_other.gameObject.name + " Left");
            }
        }

        public void Use() {
            InteractionController objectToInteract = GetNearest(this.interactables?.Where(x => x.CanUse));
            if(objectToInteract == null) return;

            objectToInteract.UseAction.Invoke();
        }

        private InteractionController GetNearest(IEnumerable<InteractionController> _from) {
            try {
                return _from?.Where(x => {
                    Vector2 direction = this.transform.gameObject.GetComponent<PlayerController>().LastMovement;
                    Vector2 objectPosition = (this.transform.position - x.InteractionPoint.position).normalized;

                    if (Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.y)) {
                        if ((direction.x > 0 && objectPosition.x < 0) || (direction.x < 0 && direction.x > 0)) return true;
                    } else {
                        if ((direction.y > 0 && objectPosition.y < 0) || (direction.y < 0 && direction.y > 0)) return true;
                    }

                    return false;
                })?.OrderBy(x => {
                    return (this.transform.position - x.InteractionPoint.position).magnitude;
                })?.First();
            } catch { }

            return null;
        }
    }
}
*/