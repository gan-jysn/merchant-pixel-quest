using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour, IInteractable {
    [SerializeField] ShopKeeperType type;

    public event Action OnInteract;

    private int spriteTypeIndex = 0;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        spriteTypeIndex = (int) type;
        animator.SetInteger("SpriteType", spriteTypeIndex);
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
