using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {
    [SerializeField] float raycastDistance = 10f;
    [SerializeField] MovementController movementController;

    private Direction dir = Direction.Down;
    private RaycastHit2D[] hit;

    private void Start() {
        if (movementController == null) {
            movementController = this.gameObject.GetComponent<MovementController>();
        }

        AddEventCallbacks();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void Update() {
        if (!GameManager.Instance.IsGamePaused) {
            if (movementController != null) {
                dir = movementController.CurrentDirection;
            }
        }
    }

    private void AddEventCallbacks() {
        //Subscribe to Event Callbacks
        InputManager.Instance.OnInteractPressed += Interact;
    }

    private void RemoveEventCallbacks() {
        //Unsubscribe to Event Callbacks
        InputManager.Instance.OnInteractPressed -= Interact;
    }

    private Vector2 GetDirection() {
        switch (dir) {
            case Direction.Up:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;
            case Direction.Left:
                return Vector2.left;
            case Direction.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    private void Interact() {
        if (GameManager.Instance.IsGamePaused)
            return;

        //Add Interact Functionality
        Vector2 direction = GetDirection();
        hit = Physics2D.RaycastAll(transform.position, direction, raycastDistance);
        GameObject obj = null;
        if (hit != null) {
            foreach (RaycastHit2D raycast in hit) {
                if (raycast.collider.CompareTag("Interactable")) {
                    obj = raycast.collider.gameObject;
                    break;
                }
            }
        }

        if (obj != null) {
            Debug.Log("Interact");
            obj.GetComponent<IInteractable>().Interact();
        }
    }
}
