using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller {
    public class ArrowController : EntityBaseController {
        private Vector2 direction;
        public void MoveDirection(Vector2 _dir) {
            this.direction = _dir;
        }

        protected override void Behaviour() {
            base.Behaviour();

            this.Move(this.direction);
        }
    }
}