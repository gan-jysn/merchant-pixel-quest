using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int hp;
    [SerializeField] int maxHP = 60;

    public int HealthPoints { get { return hp; } }

    #region Events
    public event Action OnTakeDamage;
    public event Action OnDeath;
    #endregion

    private void Start() {
        ResetHP();
    }

    private void ResetHP() {
        hp = maxHP;
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        OnTakeDamage?.Invoke();

        if (hp <= 0) {
            OnDeath?.Invoke();
        }
    }
}
