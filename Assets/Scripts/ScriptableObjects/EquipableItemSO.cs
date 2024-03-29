using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Equipable Item")]
public class EquipableItemSO : ItemSO, IEquipable {
    public EquipType EquipType;
    public Sprite SpriteSheet;
    public RuntimeAnimatorController AnimController;

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
