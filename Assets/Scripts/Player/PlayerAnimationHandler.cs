using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimHandler : MonoBehaviour {
    [SerializeField] MovementController movementController;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();

        if (movementController == null) {
            movementController = GetComponentInParent<MovementController>();
        }

        AddEventCallbacks();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void Update() {
        if (movementController != null) {
            if (!movementController.IsMovementEnabled)
                return;
        }

        UpdateMovementVariables();
    }

    private void AddEventCallbacks() {
        if (movementController != null) {
            movementController.OnSetLastDirection += SetDirection;
            movementController.OnAttack += TriggerAttack;
            movementController.OnRunToggle += SetRunToggleValue;
        }
    }

    private void RemoveEventCallbacks() {
        if (movementController != null) {
            movementController.OnSetLastDirection += SetDirection;
            movementController.OnAttack -= TriggerAttack;
            movementController.OnRunToggle -= SetRunToggleValue;
        }
    }

    private void SetDirection(Direction dir) {
        float index = (float) dir;
        animator.SetFloat("LastDirection", index);
    }

    private void TriggerAttack() {
        //Implement Attack
    }

    private void SetRunToggleValue(bool isRunEnabled) {
        animator.SetBool("IsRunEnabled", isRunEnabled);
    }

    private void UpdateMovementVariables() {
        if (movementController != null) {
            Vector2 movement = movementController.MoveVector;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }
}
