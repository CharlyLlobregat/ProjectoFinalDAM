using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller {
    public class ItemController : EntityBaseController {
        private float deltaAlive = 10;
        protected override void Behaviour() {
            base.Behaviour();

            if(this.deltaAlive < 0) this.Kill();
            deltaAlive -= Time.deltaTime;

        }
    }
}