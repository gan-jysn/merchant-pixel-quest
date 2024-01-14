using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour, IInteractable {
    [SerializeField] SignSO signData;
    [SerializeField] GameObject interactableNotice;
    [SerializeField] bool isInteractable = false;

    public bool IsInteractable { 
        get { 
            return isInteractable; 
        } 
        set { 
            isInteractable = value;
            if (interactableNotice != null) {
                interactableNotice.SetActive(value);
            }
        } 
    }

    public void Interact() {
        if (!isInteractable)
            return;

        //Call DialogueManager
        if (signData == null)
            return;

        DialogueManager.Instance.ShowDialogue(signData.signDescription);
    }
}
