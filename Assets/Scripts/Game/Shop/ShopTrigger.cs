using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopTrigger : MonoBehaviour {
    public event Action OnPlayerEntered;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isTriggered) {
            OnPlayerEntered?.Invoke();
            isTriggered = true;
            StartCoroutine(TriggerTimer());
        }
    }

    private IEnumerator TriggerTimer() {
        yield return new WaitForSeconds(0.5f);
        isTriggered = false;
    }
}
