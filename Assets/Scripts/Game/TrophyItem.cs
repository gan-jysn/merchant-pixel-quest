using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class TrophyItem : MonoBehaviour {
    [SerializeField] TrophyItemSO itemData;

    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = itemData.Icon;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            InventoryHandler inventory = collision.GetComponent<InventoryHandler>();
            inventory.StoreItem(itemData);

            Destroy(gameObject);
        }
    }
}
