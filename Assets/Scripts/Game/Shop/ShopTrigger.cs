using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopTrigger : MonoBehaviour {

    public event Action OnPlayerEntered;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (CompareTag("Player")) {
            OnPlayerEntered?.Invoke();
        }
    }
}
