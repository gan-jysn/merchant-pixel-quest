using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NoticeTrigger : MonoBehaviour {
    [SerializeField] GameObject notice;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            notice.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            notice.SetActive(false);
        }
    }
}
