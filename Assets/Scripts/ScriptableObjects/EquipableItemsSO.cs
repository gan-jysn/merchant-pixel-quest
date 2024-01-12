using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Equipable Item")]
public class EquipableItemSO : ItemSO, IEquipable {
    public EquipType EquipType;
    public Sprite SpriteSheet;
    public GameObject ItemPrefab;
    public SpriteLibraryAsset itemAsset;

    public void Equip() {

    }

    public void Unequip() {

    }
}

public enum EquipType {
    Hat,
    Body,
    Weapon
}
