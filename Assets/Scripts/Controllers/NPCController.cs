using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
    public float Speed = 5.0f;
    public Vector2 LastMovement = Vector2.zero;

    public float DeltaBetweenSteps;
    public float DeltaStep;

    public Vector2 Direction;
    private bool walking = false;
    private bool attacking = false;


    private bool isMoving = false;
    public float deltaStepsCounter;
    public float deltaStepCounter;


    public float AttackTime = 0.5f;
    private float attackTimerCounter;
    public bool IsAttaking;

    public bool CanMove = true;

    private Animator animator;
    private Rigidbody2D rigidBody;

    private void Start() {
        this.deltaStepsCounter = DeltaBetweenSteps;
        this.deltaStepCounter = DeltaStep;
        this.animator = this.transform.Find("NPCSprite").GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (this.isMoving) {
            this.deltaStepCounter -= Time.deltaTime;
            this.MovePosition(Direction.x, Direction.y);

            if (this.deltaStepCounter < 0) {
                this.isMoving = false;
                this.deltaStepsCounter = DeltaBetweenSteps;
                this.Direction = Vector2.zero;
                this.MovePosition(0, 0);
            }
        } else {
            this.deltaStepsCounter -= Time.deltaTime;

            if (this.deltaStepsCounter < 0) {
                this.isMoving = true;
                this.deltaStepCounter = DeltaStep;
                Direction = new Vector2(
                    Random.Range(-1, 2),
                    Random.Range(-1, 2)
                );
            }

            this.MovePosition(0, 0);
        }
    }

    private void LateUpdate() {
        this.MoveAnimate(Direction.x, Direction.y);
    }

    private void MovePosition(float _horizontal, float _vertical) {
        this.rigidBody.velocity = new Vector2(
                Mathf.Abs(_horizontal) < 0.2f ? 0 : (_horizontal * this.Speed * 100 * Time.deltaTime),
                Mathf.Abs(_vertical) < 0.2f ? 0 : (_vertical * this.Speed * 100 * Time.deltaTime)
            ).normalized * Speed;

        if (Mathf.Abs(_horizontal) > 0.2f || Mathf.Abs(_vertical) > 0.2f)
            this.LastMovement = new Vector2(_horizontal, _vertical);
    }

    private void MoveAnimate(float _horizontal, float _vertical) {
        this.walking = Mathf.Abs(_horizontal) > 0.2f || Mathf.Abs(_vertical) > 0.2f;

        //Animator Update || Actualizamos el animator
        this.animator.SetBool("Walking", this.walking);
        this.animator.SetFloat(GameConstants.AXIS_H, _horizontal);
        this.animator.SetFloat(GameConstants.AXIS_V, _vertical);
        this.animator.SetFloat("LastH", this.LastMovement.x);
        this.animator.SetFloat("LastV", this.LastMovement.y);
    }
}