using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sign_Trigger : MonoBehaviour, IInteractable {
    [SerializeField] SignSO signData;
    [SerializeField] bool isInteractable = false;
    
    public void Interact() {
        if (!isInteractable)
            return;

        //Call DialogueManager
        if (signData == null)
            return;


    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision != null && collision.CompareTag("Player")) {
            isInteractable = false;
        }
    }
}
