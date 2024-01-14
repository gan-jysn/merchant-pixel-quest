using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopTrigger : MonoBehaviour {
    [SerializeField] Animator doorAnimator;

    public event Action OnPlayerEntered;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isTriggered) {
            isTriggered = true;
            StartCoroutine(TriggerTimer());
        }
    }

    private IEnumerator TriggerTimer() {
        if (doorAnimator != null) {
            doorAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(0.3f);
        }

        OnPlayerEntered?.Invoke();
        yield return new WaitForSeconds(0.5f);
        isTriggered = false;
    }
}
