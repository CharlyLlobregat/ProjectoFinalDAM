using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Stats;

namespace Controller {
    public class EnemyController : EntityBaseController {
        public List<Vector3> InterestPoints;
        public uint currentInterest;

        private Vector3 currentInterestPoint;

        public float DetectionRange = 1.5f;
        public float ChaseRange = 3f;
        public float AttackRange = 0.3f;

        public bool Ranged;

        public bool detected;
        public bool chasing;
        public bool shouldMove = true;

        protected override void Behaviour() {
            base.Behaviour();

            if ((this.transform.position - PlayerController.Instance.transform.position).magnitude < DetectionRange)
                detected = chasing = true;
            if ((this.transform.position - PlayerController.Instance.transform.position).magnitude > ChaseRange)
                detected = chasing = false;

            if (this.detected || this.chasing) this.currentInterestPoint = PlayerController.Instance.transform.position;
            else this.currentInterestPoint = InterestPoints[(int)this.currentInterest];

            if (this.Ranged) {
                this.shouldMove = this.inv.Items.Any(x => x.Amount == 0 && x.Item.Weapon == ItemStats.WeaponType.Arrow);
            }else
                shouldMove = (this.transform.position - this.currentInterestPoint).magnitude > (this.detected || this.chasing ? this.AttackRange : 0.1f);

            if(this.chasing || this.detected) {
                if(this.interact.CanMove && this.shouldMove)
                    MoveTo(new Vector2(
                        this.currentInterestPoint.x,
                        this.currentInterestPoint.y
                    ));
                else
                    Move(Vector2.zero);
            }else{
                if (this.interact.CanMove && this.shouldMove)
                    MoveTo(new Vector2(
                        this.currentInterestPoint.x,
                        this.currentInterestPoint.y
                    ));
                else if(!this.shouldMove) this.currentInterest++;
            }


            if (this.interact.CanAttack)
                if (this.Ranged){
                    if((this.transform.position - this.currentInterestPoint).magnitude < this.AttackRange &&
                        GetComponent<Interaction.InteractionManager>().Interactables.Exists(x => x.GetComponent<Stats.EntityStats>().IsPlayer))
                            this.Attack();
                } else if(GetComponent<Interaction.InteractionManager>().Interactables.Exists(x => x.GetComponent<Stats.EntityStats>().IsPlayer))
                    this.Attack();

            if (this.currentInterest >= InterestPoints.Count)
                this.currentInterest = 0;
        }

        public new void OnLoad(BinaryReader _reader) {
            this.currentInterest = _reader.ReadUInt32();
            this.detected = _reader.ReadBoolean();
            this.chasing = _reader.ReadBoolean();
            this.shouldMove = _reader.ReadBoolean();
        }

        public new void OnSave(BinaryWriter _writer) {
            _writer.Write(this.currentInterest);
            _writer.Write(this.detected);
            _writer.Write(this.chasing);
            _writer.Write(this.shouldMove);
        }
    }
}