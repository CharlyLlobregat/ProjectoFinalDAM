using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InteractionManager))]
[RequireComponent(typeof(Stats.EntityStats))]
[RequireComponent(typeof(Inventory))]
public class BaseController : MonoBehaviour {
    protected Inventory inv;
    protected Rigidbody2D rigidBody;
    protected Animator animator;
    protected InteractionManager interact;
    protected InteractionController interaction;
    protected Stats.EntityStats stats;

    public Vector2 LastMovement;

    private float attackTimerCounter;

    private void Awake() {
        this.inv = GetComponent<Inventory>();
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponentInChildren<Animator>();
        this.interact = GetComponent<InteractionManager>();
        this.interaction = GetComponent<InteractionController>();
        this.stats = GetComponent<Stats.EntityStats>();

        this.OnAttack = this.OnAttack ?? new BaseController.OnAttackEvent();
        this.OnPick = this.OnPick ?? new BaseController.OnPickEvent();
        OnAwake();
    }

    private void Update() => Behaviour();
    private void LateUpdate() => LateBehaviour();

    protected void Move(Vector2 _direction) {
        // Movement
        this.rigidBody.velocity = new Vector2(
                Mathf.Abs(_direction.x) < 0.2f ? 0 : (_direction.x * this.stats.Speed * 100 * Time.deltaTime),
                Mathf.Abs(_direction.y) < 0.2f ? 0 : (_direction.y * this.stats.Speed * 100 * Time.deltaTime)
            ).normalized * this.stats.Speed;

        if (Mathf.Abs(_direction.x) > 0.2f || Mathf.Abs(_direction.y) > 0.2f)
            LastMovement = _direction;

        // Animation
        this.interaction.IsMoving = Mathf.Abs(_direction.x) > 0.2f || Mathf.Abs(_direction.y) > 0.2f;

        MoveAnimation();
        OnMove?.Invoke(_direction);
    }
    protected void MoveAnimation() {
        //Animator Update || Actualizamos el animator
        this.animator.SetBool("Walking", this.interaction.IsMoving);
        this.animator.SetFloat(GameConstants.AXIS_H, LastMovement.x);
        this.animator.SetFloat(GameConstants.AXIS_V, LastMovement.y);
        this.animator.SetFloat("LastH", LastMovement.x);
        this.animator.SetFloat("LastV", LastMovement.y);
    }
    protected void MoveTo(Vector3 _objective) {
        this.Move(-(this.transform.position - _objective).normalized);

        OnMoveTo?.Invoke(_objective);
    }
    protected void Attack(bool _attack) {
        if (this.interaction.IsAttacking) {
            this.attackTimerCounter -= Time.deltaTime;

            if (this.attackTimerCounter < 0) {
                this.interaction.IsAttacking = false;
                this.animator.SetBool(GameConstants.ATTACK, false);
            }
        } else {
            if (_attack) {
                this.interaction.IsAttacking = true;
                this.attackTimerCounter = 0.5f;
                this.rigidBody.velocity = Vector2.zero;
                this.animator.SetBool(GameConstants.ATTACK, true);
                OnAttack?.Invoke();
            }
        }
    }
    protected virtual void Pick() {
        OnPick?.Invoke(this.interact.Pick());
    }
    protected virtual void UseItem() {
        OnUseItem?.Invoke(this.interact.UseItem());
    }



    protected virtual void Behaviour() { }
    protected virtual void LateBehaviour() { }
    protected virtual void OnAwake() { }

    protected OnMoveEvent OnMove;
    protected OnMoveToEvent OnMoveTo;
    protected OnAttackEvent OnAttack;
    protected OnPickEvent OnPick;
    protected OnUseItemEvent OnUseItem;

    public class OnMoveEvent : UnityEvent<Vector2>{ }
    public class OnMoveToEvent : UnityEvent<Vector3> { }
    public class OnAttackEvent : UnityEvent {}
    public class OnPickEvent : UnityEvent<Stats.ItemStats>{ }
    public class OnUseItemEvent : UnityEvent<Stats.ItemStats> { }
}