using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats.EntityStats))]
public class EnemyController : MonoBehaviour {
    public float Speed = 1f;
    public float DeltaBetweenSteps;
    public float DeltaStep;

    public Vector2 Direction;

    private Rigidbody2D rigidBody2d;
    private bool isMoving = false;
    public float deltaStepsCounter;
    public float deltaStepCounter;

    private void Start() {
        this.rigidBody2d = GetComponent<Rigidbody2D>();
        this.deltaStepsCounter = DeltaBetweenSteps;
        this.deltaStepCounter = DeltaStep;
    }

    private void Update() {
        if (this.isMoving) {
            this.deltaStepCounter -= Time.deltaTime;
            this.rigidBody2d.velocity = Direction * Speed * 100 * Time.deltaTime;

            if(this.deltaStepCounter < 0) {
                this.isMoving = false;
                this.deltaStepsCounter = DeltaBetweenSteps;
                this.rigidBody2d.velocity = Vector2.zero;
            }
        } else {
            this.deltaStepsCounter -= Time.deltaTime;

            if(this.deltaStepsCounter < 0) {
                this.isMoving = true;
                this.deltaStepCounter = DeltaStep;
                Direction = new Vector2(
                    Random.Range(-1, 2),
                    Random.Range(-1, 2)
                );
            }
        }
    }
}