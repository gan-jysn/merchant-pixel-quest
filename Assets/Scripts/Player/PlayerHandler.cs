using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, ITakeDamage {
    private float health;

    public float Health {
        get {
            return health;
        }
        set {
            health = value;

            if (health <= 0) {
                OnDeath?.Invoke();
            }
        }
    }

    #region Events
    public event Action OnDeath;
    #endregion


    public void TakeDamage(float damage) {
        Health -= damage;
    }
}
