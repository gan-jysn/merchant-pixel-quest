using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject {
    public int ItemID;
    public string ItemName;
    [TextArea] public string Description;
    public int Value;
    public ItemType Type;
    public Sprite Icon;
}

public enum ItemType {
    Clothing,
    Consumable,
    Trophy
}