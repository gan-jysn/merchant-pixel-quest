using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SimulatedAttack : MonoBehaviour {
    [SerializeField] MovementController controller;
    [SerializeField] AttackDir attackDirection;
    [SerializeField] float fixedDamage = 20f;
    
    Collider2D attackHitbox;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Vector2 rightOffset = new Vector2(0.85f, 0.15f);

    private void Start() {
        attackHitbox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        controller.OnAttack += Attack;
        controller.OnHorizontalDirectionChanged += UpdateDirection;
    }

    public void Attack() {
        animator.SetTrigger("Attack");
        SoundManager.Instance.PlayAttackSFX();
        switch (attackDirection) {
            case AttackDir.Left:
                transform.localPosition = new Vector3(-rightOffset.x, rightOffset.y);
                break;
            case AttackDir.Right:
                transform.localPosition = rightOffset;
                break;
        }
    }

    public void EnableAttack() {
        controller.IsAttackEnabled = true;
    }

    public void DisableAttack() {
        controller.IsAttackEnabled = false;
    }

    private void UpdateDirection(bool dir) {
        attackDirection = dir ? AttackDir.Right : AttackDir.Left;

        switch (attackDirection) {
            case AttackDir.Right:
                spriteRenderer.flipX = false;
                break;
            case AttackDir.Left:
                spriteRenderer.flipX = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<ITakeDamage>().TakeDamage(fixedDamage);
        }
    }
}

public enum AttackDir {
    Left,
    Right
}
