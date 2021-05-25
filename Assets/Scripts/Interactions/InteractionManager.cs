using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Stats;

namespace Interaction {
    /**
     * Se encarga de determianar contra que entidad realizar la acción y
     * proporcionar los datos suficientes para realizar esta.
     */
    [RequireComponent(typeof(Collider2D))]
    public class InteractionManager : MonoBehaviour {
        public List<InteractionController> Interactables;

        public uint LevelToWin;
        public void OnAttack(uint _damage) {
            GetNearest(this.Interactables.Where(x => x.CanBeAttacked))?.OnAttacked?.Invoke(GetComponent<EntityStats>(), _damage);
        }

        public void OnPick() {
            GetNearest(this.Interactables.Where(x => x != null && x.CanBePicked))?.OnPicked?.Invoke(GetComponent<EntityStats>());
        }
        public void OnTalk() {
            GetNearest(this.Interactables.Where(x => x.CanBeTalked))?.OnTalked?.Invoke(GetComponent<EntityStats>());
        }

        public void OnUse(ItemStats _item) {
            _item?.GetComponent<InteractionController>()?.OnUsed?.Invoke();
        }
        public void OnPlace(ItemStats _item, Vector3 _position) {
            _item?.GetComponent<InteractionController>()?.OnPlaced?.Invoke(_position);
        }


        public void ShowWin(uint _lvl) {
            if (_lvl == LevelToWin)
                UIManager.Instance.ShowGameOver(true);
        }
        private void OnTriggerEnter2D(Collider2D _other) {
            if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
                this.Interactables.Add(temp);
        }
        private void OnTriggerExit2D(Collider2D _other) {
            if (_other.TryGetComponent<InteractionController>(out InteractionController temp))
                this.Interactables.Remove(temp);
        }

        public InteractionController GetNearest(IEnumerable<InteractionController> _from) {
            try{
                return _from?.Where(x => {
                    Vector2 direction = default;
                    if(TryGetComponent<Controller.PlayerController>(out Controller.PlayerController pCon))
                        direction = pCon.LastMovement;
                    else if(TryGetComponent<Controller.EnemyController>(out Controller.EnemyController eCon))
                        direction = eCon.LastMovement;
                    else if(TryGetComponent<Controller.ArrowController>(out Controller.ArrowController aCon))
                        direction = aCon.LastMovement;
                    Vector2 objectPosition = (this.transform.position - x.InteractionPoint.position).normalized;

                    if (Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.y)) {
                        if ((direction.x > 0 && objectPosition.x < 0) || (direction.x < 0 && objectPosition.x > 0)) return true;
                    } else {
                        if ((direction.y > 0 && objectPosition.y < 0) || (direction.y < 0 && objectPosition.y > 0)) return true;
                    }

                    return false;
                })
                ?.Where(x => x.IsPlayer == !GetComponent<InteractionController>().IsPlayer)
                ?.OrderBy(x => (this.transform.position - x.InteractionPoint.position).magnitude)
                ?.First();
            } catch (System.InvalidOperationException) {
                return null;
            }
        }
    }
}