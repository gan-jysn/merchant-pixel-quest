using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sign_Trigger : MonoBehaviour {
    Sign sign;

    private void Start() {
        sign = GetComponentInParent<Sign>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            sign.IsInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            sign.IsInteractable = false;
        }
    }
}
