using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    private static PlayerController Instance;
    public static PlayerController GetInstance() => Instance;

    public float Speed = 5.0f;
    public Vector2 LastMovement = Vector2.zero;

    public static bool PlayerCreated = false;
    private bool walking = false;
    private bool attacking = false;

    private Animator animator;
    private Rigidbody2D rigidBody;

    public float AttackTime = 0.5f;
    private float attackTimerCounter;
    public bool IsAttaking;

    public bool CanMove = true;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        this.animator = this.transform.Find("PlayerSprite").GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        this.walking = false;

        float horizontal = Input.GetAxisRaw(GameConstants.AXIS_H);
        float vertical = Input.GetAxisRaw(GameConstants.AXIS_V);

        if(Input.GetMouseButtonDown(0)){
            this.gameObject.GetComponent<InteractionManager>().Activate();
            this.gameObject.GetComponent<InteractionManager>().Pick();
        }

        if(this.CanMove)    this.Attack(Input.GetMouseButtonDown(0));
        if(this.CanMove)    this.MovePosition(horizontal, vertical);
        else                this.MovePosition(0, 0);
    }

    private void LateUpdate() {
        float horizontal = Input.GetAxisRaw(GameConstants.AXIS_H);
        float vertical = Input.GetAxisRaw(GameConstants.AXIS_V);

        this.MoveAnimate(horizontal, vertical);
    }

    private void MovePosition(float _horizontal, float _vertical) {
        this.rigidBody.velocity = new Vector2(
                Mathf.Abs(_horizontal) < 0.2f ? 0 : (_horizontal * this.Speed * 100 * Time.deltaTime),
                Mathf.Abs(_vertical) < 0.2f ? 0 : (_vertical * this.Speed * 100 * Time.deltaTime)
            ).normalized * Speed;

        if(Mathf.Abs(_horizontal) > 0.2f || Mathf.Abs(_vertical) > 0.2f)
            this.LastMovement = new Vector2(_horizontal, _vertical);
    }

    private void MoveAnimate(float _horizontal, float _vertical) {
        if(Mathf.Abs(_horizontal) > 0.2f || Mathf.Abs(_vertical) > 0.2f)    this.walking = true;
        else                                                                this.walking = false;

        //Animator Update || Actualizamos el animator
        this.animator.SetBool("Walking", this.walking);
        this.animator.SetFloat(GameConstants.AXIS_H, _horizontal);
        this.animator.SetFloat(GameConstants.AXIS_V, _vertical);
        this.animator.SetFloat("LastH", this.LastMovement.x);
        this.animator.SetFloat("LastV", this.LastMovement.y);
    }

    private void Attack(bool _attack) {
        if (this.attacking) {
            this.attackTimerCounter -= Time.deltaTime;

            if(this.attackTimerCounter < 0) {
                this.attacking = this.IsAttaking = false;
                this.animator.SetBool(GameConstants.ATTACK, false);
            }
        }else{
            if(_attack){
                this.attacking = this.IsAttaking = true;
                this.attackTimerCounter = AttackTime;
                this.rigidBody.velocity = Vector2.zero;
                this.animator.SetBool(GameConstants.ATTACK, true);
            }
        }
    }
}
