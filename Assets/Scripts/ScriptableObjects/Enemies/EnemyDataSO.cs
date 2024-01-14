using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject {
    public int ID;
    public float Health;
    public string Name;
    public List<ItemDropRate> ItemDrops = new List<ItemDropRate>();
}

[System.Serializable]
public class ItemDropRate {
    public GameObject ItemPrefab;
    public float DropPercentage;
}