using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Controller {
    public class ArrowController : EntityBaseController {
        private Vector2 direction;
        private Vector3 startPosition;

        public void MoveDirection(Vector2 _dir) {
            this.direction = _dir;
        }

        public new void OnLoad(BinaryReader _reader) {
            this.direction = new Vector2(_reader.ReadSingle(), _reader.ReadSingle());

            this.transform.position = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), 0);

            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _reader.ReadSingle()));

            this.startPosition = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), 0);
        }

        public new void OnSave(BinaryWriter _writer) {
            _writer.Write(this.direction.x);
            _writer.Write(this.direction.y);

            _writer.Write(this.transform.position.x);
            _writer.Write(this.transform.position.y);

            _writer.Write(this.transform.rotation.eulerAngles.z);

            _writer.Write(this.startPosition.x);
            _writer.Write(this.startPosition.x);
        }

        protected override void Behaviour() {
            base.Behaviour();

            this.Move(this.direction);
            if(TryGetComponent<Interaction.InteractionManager>(out Interaction.InteractionManager _intMan)) {
                var it = _intMan.GetNearest(_intMan.Interactables.Where(x => x.CanBeAttacked));
                if(it != null) {
                        it?.OnAttacked?.Invoke(
                        GetComponent<Stats.EntityStats>(),
                        this.stats.Strength + this.inv.Equiped.First().Item.Strength
                    );
                    this.Kill();
                }
            }

            if((this.startPosition - this.transform.position).magnitude > this.stats.Health)
                this.Kill();
        }

        protected override void OnAwake() {
            base.OnAwake();

            this.startPosition = this.transform.position;
        }
    }
}