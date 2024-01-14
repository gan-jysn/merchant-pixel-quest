using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<AreaEnemySO> areaEnemyData = new List<AreaEnemySO>();
    [SerializeField] float spawnDelay = 5;
}
