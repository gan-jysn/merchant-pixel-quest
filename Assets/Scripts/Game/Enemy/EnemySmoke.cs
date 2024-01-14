using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmoke : MonoBehaviour {
    Enemy enemyReference;

    private void Start() {
        enemyReference = GetComponentInParent<Enemy>();
    }

    public void DestroyEnemyObject() {
        if (enemyReference != null) {
            enemyReference.DestroyObj();
        }
    }
}
