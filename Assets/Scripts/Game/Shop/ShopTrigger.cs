using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour, IInteractable {

    public event Action OnInteract;

    public void Interact() {
        OnInteract?.Invoke();
    }
}
