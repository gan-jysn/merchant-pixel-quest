using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        smokeAnimator.gameObject.SetActive(true);
        smokeAnimator.SetTrigger("Pop");

        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
