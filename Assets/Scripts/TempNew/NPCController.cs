using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Stats;

namespace Controller {
    public class NPCController : EntityBaseController {
        public List<Vector3> InterestPoints;
        public uint currentInterest;

        private Vector3 currentInterestPoint;
        public bool shouldMove = true;

        protected override void Behaviour() {
            base.Behaviour();

            if(InterestPoints.Count > 0){
                this.currentInterestPoint = InterestPoints[(int)this.currentInterest];
                shouldMove = (this.transform.position - this.currentInterestPoint).magnitude > 0.1f;

                if (this.interact.CanMove && this.shouldMove)
                    MoveTo(new Vector2(
                        this.currentInterestPoint.x,
                        this.currentInterestPoint.y
                    ));
                else if (!this.shouldMove) this.currentInterest++;

                if (this.currentInterest >= InterestPoints.Count)
                    this.currentInterest = 0;
            }
        }

        public new void OnLoad(BinaryReader _reader) {
            this.currentInterest = _reader.ReadUInt32();
            this.shouldMove = _reader.ReadBoolean();
        }

        public new void OnSave(BinaryWriter _writer) {
            _writer.Write(this.currentInterest);
            _writer.Write(this.shouldMove);
        }
    }
}