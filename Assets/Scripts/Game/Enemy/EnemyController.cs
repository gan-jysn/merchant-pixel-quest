using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {
    [SerializeField] float speed;

    private Rigidbody2D rb;
    private Vector2 targetDir;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
}
