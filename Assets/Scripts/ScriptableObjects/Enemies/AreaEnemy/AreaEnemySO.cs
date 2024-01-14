using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/AreaEnemySO")]
public class AreaEnemySO : ScriptableObject {
    public string AreaName;
    public int MaxEnemiesInArea;
    public List<GameObject> EnemyTypes = new List<GameObject>();
    public List<Transform> SpawnPoints = new List<Transform>();
}
