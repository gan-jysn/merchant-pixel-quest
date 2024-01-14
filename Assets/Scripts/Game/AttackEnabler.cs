using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackEnabler : MonoBehaviour {
    [SerializeField] bool isEnabled = true;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<MovementController>().IsAttackEnabled = isEnabled;
        }
    }
}
