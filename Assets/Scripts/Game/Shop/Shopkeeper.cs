using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour, IInteractable {
    [SerializeField] ShopKeeperType type;

    public event Action OnInteract;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.SetInteger("SpriteType", (int) type);
    }

    public void Interact() {
        OnInteract?.Invoke();
    }
}

public enum ShopKeeperType {
    OldWoman,
    OldMan,
    Smithy
}
