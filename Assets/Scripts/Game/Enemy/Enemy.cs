using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour, ITakeDamage {
    [SerializeField] EnemyDataSO data;
    [SerializeField] Animator smokeAnimator;

    private float health;

    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            
            if (health <= 0) {
                OnEnemyDeath?.Invoke();
            }
        }
    }
    
    #region Events
    public event Action OnEnemyDeath;
    #endregion

    private void Start() {
        if (data != null) {
            health = data.Health;
        }

        OnEnemyDeath += SpawnDrops;
    }

    public void TakeDamage(float damage) {
        Health -= damage;
        Debug.Log(gameObject.name + " received " + damage.ToString() + " damage.");
    }

    private void SpawnDrops() {
        //Add Commulative Probability Formula
        int dropIndex = Random.Range(0, data.ItemDrops.Count);
        GameObject obj = Instantiate(data.ItemDrops[dropIndex].ItemPrefab, transform.position, Quaternion.identity, transform.parent);

        smokeAnimator.gameObject.SetActive(true);
        smokeAnimator.SetTrigger("Pop");
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
