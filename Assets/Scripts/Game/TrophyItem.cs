using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class TrophyItem : MonoBehaviour {
    [SerializeField] TrophyItemSO itemData;

    SpriteRenderer spriteRenderer;
    Collider2D collider;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        spriteRenderer.sprite = itemData.Icon;
    }


}
