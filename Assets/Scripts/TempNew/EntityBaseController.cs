using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;
using Interaction;
using Inventory;
using System.Linq;
using System.IO;

namespace Controller {
    /**
     * El controlador base para las entidades 'vivas'.
     * Se encarga de controllar cuando se va a realizar alguna acción y
     * asegurarse de que se tienen los elementos necesarios para el correcto funcionamiento de la entidad.
     */

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Inventory.Inventory))]
    [RequireComponent(typeof(EntityStats))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Interaction.InteractionController))]
    [RequireComponent(typeof(Interaction.InteractionManager))]
    [RequireComponent(typeof(Animator))]
    public class EntityBaseController : MonoBehaviour, ISave {
        protected Inventory.Inventory inv;
        protected Rigidbody2D rigidBody;
        protected EntityStats stats;
        protected Interaction.InteractionController interact;
        protected Animator animator;

        [SerializeField] public Vector2 LastMovement;
        public bool ShouldActivateWeapon;
        protected bool isPlayer;

        // Acciones que se realizan sobre el propio objecto, no sobre otro.
        #region SELF_ACTION
        protected void Move(Vector2 _direction){
            uint itemSpeed = 0;
            foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
                itemSpeed += i.Speed;
            uint totalSpeed = itemSpeed + this.stats.Speed;

            // Calculo de movimiento
            this.rigidBody.velocity = new Vector2(
                    Mathf.Abs(_direction.x) < 0.2f ? 0 : (_direction.x),
                    Mathf.Abs(_direction.y) < 0.2f ? 0 : (_direction.y)
                ).normalized * totalSpeed;

            // Dirección del movimiento
            if (Mathf.Abs(_direction.x) > 0.2f || Mathf.Abs(_direction.y) > 0.2f)
                LastMovement = _direction;

            // Interacción
            this.interact.IsMoving = Mathf.Abs(_direction.x) > 0.2f || Mathf.Abs(_direction.y) > 0.2f;

            ChangeAnimation("MOVE");

            this.interact.OnMove?.Invoke(_direction);
        }
        protected void MoveTo(Vector2 _objective){
            this.Move(-(new Vector2(this.transform.position.x, this.transform.position.y) - _objective).normalized);

            this.interact.OnMoveTo?.Invoke(_objective);
        }
        protected void MoveTo(Vector3 _position) => MoveTo(new Vector2(_position.x, _position.y));
        protected void ChangeAnimation(string _animation){
            switch (_animation) {
                case "MOVE":

                    //Animator Update || Actualizamos el animator
                    this.animator.SetBool("Walking", this.interact.IsMoving);
                    this.animator.SetFloat(GameConstants.AXIS_H, LastMovement.x);
                    this.animator.SetFloat(GameConstants.AXIS_V, LastMovement.y);
                    this.animator.SetFloat("LastH", LastMovement.x);
                    this.animator.SetFloat("LastV", LastMovement.y);

                    break;
                case "ATTACK":
                    this.animator.SetBool("Attacking", this.interact.IsAttacking);
                    break;
                case "NOTATTACK":
                    this.animator.SetBool("Attacking", false);
                    break;
            }
        }
        public void Attacked(EntityStats _by, uint _damage) {
            uint itemDefense = 0;
            foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
                itemDefense += i.Defense;
            uint totalDamage = (uint)Mathf.Max(_damage - itemDefense - this.stats.Defense, 0);

            this.stats.ReduceHealth(totalDamage);
            if(this.stats.CurrentHealth == 0)
                this.interact.OnKilledBy?.Invoke(_by);

            if(SettingsManager.Instance.ShowDamageNumbers)
                if(((_by.IsPlayer || _by.Name == "Arrow") && SettingsManager.Instance.ShowEnemyDamageNumbers) || SettingsManager.Instance.ShowPlayerDamageNumbers)
                    UIManager.Instance.CreateDamageNumber(this.transform.position, totalDamage);
        }
        public void Picked(EntityStats _by) {
            if(_by.TryGetComponent<Inventory.Inventory>(out Inventory.Inventory inv)) {
                this.inv.Items.ForEach(x => {
                    for(int i = 0; i < x.Amount; i++)
                        inv.AddItem(ItemManager.Instance.GetItem(x.Item));
                });

                Destroy(this.gameObject);
            }
        }
        public void Talked(EntityStats _by) {
            if(_by.IsPlayer && TryGetComponent<Dialogue.DialogueController>(out Dialogue.DialogueController dialogue)) {
                _by.GetComponent<Interaction.InteractionController>().IsTalking = true;
                UIManager.Instance.StartDialogue(dialogue);
            }
        }
        // public void Used() { }
        public void Placed(Vector3 _position) {
            GameManager.Instance.InstantiateItem(this.GetComponent<ItemStats>(), _position);
        }
        public void Killed(EntityStats _by) {
            _by.AddExp(this.stats.EXP);
            DropInventory();
            Kill();
        }
        #endregion

        // Acciones que se realizan sobre datos del objeto
        #region DATA_ACTION
        // Se obtiene datos del inventario: Selected Item
        protected void Place() {
            ItemStats item = InventoryManager.Instance.InvSelect.ActiveToggles().First(x => x.isOn).GetComponent<ItemStats>();
            this.interact?.OnPlace?.Invoke(item, this.transform.position);
        }

        // Se obtiene datos del inventario: Selected Item
        protected void Use() {
            ItemStats item = InventoryManager.Instance.InvSelect.ActiveToggles().First(x => x.isOn).GetComponent<ItemStats>();
            this.interact?.OnUse?.Invoke(item);
        }
        #endregion

        // Acciones que se realizar a otros objectos.
        #region OTHER_ACTION
        protected void Talk(){
            this.interact?.OnTalk?.Invoke();
        }
        protected void Pick(){
            this.interact.OnPick?.Invoke();
        }
        protected void Attack(){
            if(!this.interact.IsAttacking) {
                ItemStats.WeaponType wtype = ItemStats.WeaponType.None;
                bool hasArc = false;
                try {
                    wtype = this.inv.Equiped.First(x => x.Item.Type == ItemStats.ItemType.Weapon).Item.Weapon;
                }catch{ }

                if (wtype == ItemStats.WeaponType.Sword) {
                    uint itemDamage = 0;
                    foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
                        itemDamage += i.Strength;

                    this.interact.OnAttack?.Invoke(this.stats.Strength + itemDamage);
                } else if(wtype == ItemStats.WeaponType.Arc){
                    if(this.inv.Equiped.Any(x => x.Item.Weapon == ItemStats.WeaponType.Arrow)) {
                        ItemStats arrow = this.inv.Equiped.First(x => x.Item.Weapon == ItemStats.WeaponType.Arrow).Item;
                        ItemStats arc = this.inv.Equiped.First(x => x.Item.Weapon == ItemStats.WeaponType.Arc).Item;

                        GameManager.Instance.InstantiateArrow(arrow, this.transform.position, this.LastMovement, arc.Strength, this.interact.IsPlayer);
                        this.inv.DecrementAmount(arrow);
                    }
                }else{
                    try {
                        wtype = this.inv.Equiped.First(x => x.Item.Weapon == ItemStats.WeaponType.Arrow).Item.Weapon;
                    } catch { }

                    if (wtype == ItemStats.WeaponType.Arrow) {
                        uint itemDamage = 0;
                        foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
                            itemDamage += i.Strength;

                        this.interact.OnAttack?.Invoke(this.stats.Strength + itemDamage);
                    }
                }

                if (wtype != ItemStats.WeaponType.None) {
                    this.interact.IsAttacking = true;
                    ChangeAnimation("ATTACK");
                    StartCoroutine(AttackTimeOut());
                }
            }
        }
        private IEnumerator AttackTimeOut() {
            uint itemAttackSpeed = 0;
            foreach ((Stats.ItemStats i, _) in this.inv.Equiped)
                itemAttackSpeed += i.AttackSpeed;
            uint totalAttackSpeed = itemAttackSpeed + this.stats.AttackSpeed;

            yield return new WaitForSeconds(0.85f - (totalAttackSpeed * 0.15f));
            this.interact.IsAttacking = false;
            ChangeAnimation("NOTATTACK");
        }

        protected void Activate(){
            this.interact?.OnActivate?.Invoke();
        }
        #endregion

        public void DropInventory() {
            this.inv.Items.ForEach(x => {
                GameManager.Instance.InstantiateItem(
                    x.Item,
                    this.transform.position + new Vector3(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(-0.5f, 0.5f)
                    )
                );
            });
        }

        public void ActivateWeapon(Sprite _sprite, bool _active) {
            if(ShouldActivateWeapon){
                GameObject weapon = this.transform.Find("Weapon")?.gameObject;
                weapon.SetActive(_active);
                if (weapon.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer)) renderer.sprite = _sprite;
            }
        }
        public virtual void Kill() {
            Managers.EntityManager.Instance.CurrentEntities.Remove(this.GetComponent<EntityStats>());
            Destroy(this.gameObject);
        }

        private void Awake() {
            this.inv = GetComponent<Inventory.Inventory>();
            this.rigidBody = GetComponent<Rigidbody2D>();
            this.stats = GetComponent<Stats.EntityStats>();
            this.interact = GetComponent<Interaction.InteractionController>();
            this.animator = GetComponent<Animator>();

            OnAwake();
        }
        protected virtual void OnAwake() {}

        private void Update() => Behaviour();
        private void LateUpdate() => LateBehaviour();

        protected virtual void Behaviour() {}
        protected virtual void LateBehaviour() {}

        public void OnSave(BinaryWriter _writer) {
            _writer.Write(LastMovement.x);
            _writer.Write(LastMovement.y);
        }

        public void OnLoad(BinaryReader _reader) {
            LastMovement = new Vector2(_reader.ReadSingle(), _reader.ReadSingle());
        }

    }
}