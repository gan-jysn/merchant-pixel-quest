using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] AreaEnemySO areaEnemyData;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] List<Transform> spawnLocations = new List<Transform>();

    private Transform parentTransform;
    private List<Transform> spawnLocRotation = new List<Transform>();

    private void Start() {
        parentTransform = this.transform;
        spawnLocRotation = spawnLocations;

        SpawnEnemy(areaEnemyData, areaEnemyData.MaxEnemiesInArea);
    }

    private void SpawnEnemy(AreaEnemySO data, int quantity) {
        if ((quantity + enemyList.Count) > data.MaxEnemiesInArea) {
            Debug.Log("Spawn count not available.");
            return;
        }
        
        for (int i = 0;i < quantity;i++) {
            if (spawnLocRotation.Count == 0) {
                spawnLocRotation = spawnLocations;
            }
            int spawnLocIndex = Random.Range(0, spawnLocRotation.Count);
            Vector3 pos = spawnLocations[spawnLocIndex].position;
            spawnLocRotation.RemoveAt(spawnLocIndex);

            int enemyPrefabIndex = Random.Range(0, data.EnemyTypes.Count);
            GameObject obj = Instantiate(data.EnemyTypes[enemyPrefabIndex], pos, Quaternion.identity, parentTransform);
            enemyList.Add(obj);
        }
    }
}
