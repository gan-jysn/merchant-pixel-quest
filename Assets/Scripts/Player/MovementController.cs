using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour {
    [SerializeField] bool isMovementEnabled = true;
    [SerializeField] bool isAttackEnabled = false;
    [SerializeField] float defaultMovementSpeed = 5f;
    [SerializeField] float runningMovementSpeed = 7.5f;

    private Rigidbody2D rb;
    private bool isRunEnabled = false;
    private Vector2 moveVector = Vector2.zero;
    private float movementSpeed = 0;
    private Direction currentDirection = Direction.Down;
    private Direction lastDirection = Direction.Down;

    public bool IsMovementEnabled { get { return isMovementEnabled; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public bool IsRunEnabled { get { return isRunEnabled; } }
    public Vector2 MoveVector { get { return moveVector; } }
    public Direction CurrentDirection { get { return currentDirection; } }

    #region Events
    public event Action<Direction> OnSetLastDirection;
    public event Action OnAttack;
    public event Action<bool> OnHorizontalDirectionChanged;
    public event Action<bool> OnRunToggle;
    #endregion

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = defaultMovementSpeed;

        AddEventCallbacks();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void FixedUpdate() {
        if (!isMovementEnabled)
            return;

        //Movement
        rb.MovePosition(rb.position + moveVector * movementSpeed * Time.fixedDeltaTime);
    }

    private void AddEventCallbacks() {
        //Subscribe to Event Callbacks
        InputManager.Instance.OnMovementStart += OnMovementStart;
        InputManager.Instance.OnMovementEnd += OnMovementEnd;
        InputManager.Instance.OnShiftStarted += OnRunEnabled;
        InputManager.Instance.OnShiftEnded += OnRunDisabled;
        InputManager.Instance.OnAttackPressed += Attack;

        GameManager.Instance.OnGamePaused += DisableMovement;
        GameManager.Instance.OnGameResumed += EnableMovement;
    }

    private void RemoveEventCallbacks() {
        //Unsubscribe to Event Callbacks
        InputManager.Instance.OnMovementStart -= OnMovementStart;
        InputManager.Instance.OnMovementEnd -= OnMovementEnd;
        InputManager.Instance.OnShiftStarted -= OnRunEnabled;
        InputManager.Instance.OnShiftEnded -= OnRunDisabled;
        InputManager.Instance.OnAttackPressed -= Attack;

        GameManager.Instance.OnGamePaused -= DisableMovement;
        GameManager.Instance.OnGameResumed -= EnableMovement;
    }

    private void OnMovementStart(Vector2 input) {
        //Assign Input Value to Local Move Vector
        moveVector = input;
        currentDirection = CheckDirection();
    }

    private void OnMovementEnd() {
        CheckLastDirection();
        moveVector = Vector2.zero;
    }

    private Direction CheckDirection() {
        if (moveVector.x == 0) {
            if (moveVector.y > 0) {
                return Direction.Up;
            } else {
                return Direction.Down;
            }
        } else if (moveVector.y == 0) {
            if (moveVector.x > 0) {
                OnHorizontalDirectionChanged?.Invoke(true);
                return Direction.Right;
            } else {
                OnHorizontalDirectionChanged?.Invoke(false);
                return Direction.Left;
            }
        }
        return Direction.Down;
    }

    private void CheckLastDirection() {
        lastDirection = CheckDirection();
        OnSetLastDirection?.Invoke(lastDirection);
    }

    private void Attack() {
        if (!isActiveAndEnabled)
            return;

        DisableMovement();
        OnAttack?.Invoke();
        StartCoroutine(DelayEnabled(0.2f));
    }

    private IEnumerator DelayEnabled(float delay) {
        yield return new WaitForSeconds(delay);
        EnableMovement();
    }

    private void OnRunEnabled() {
        isRunEnabled = true;
        movementSpeed = runningMovementSpeed;
        OnRunToggle?.Invoke(isRunEnabled);
    }

    private void OnRunDisabled() {
        isRunEnabled = false;
        movementSpeed = defaultMovementSpeed;
        OnRunToggle?.Invoke(isRunEnabled);
    }

    private void DisableMovement() {
        isMovementEnabled = false;
    }

    public void EnableMovement() {
        isMovementEnabled = true;
    }
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}