using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class DialogueManager : Singleton<DialogueManager> {
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueTxt;

    [Header("Settings")]
    [SerializeField] float typingSpeed = 20.0f;
    [SerializeField] float skipSpeed = 5.0f;

    private string tempTxt;
    private bool isSkipping;
    private bool isDialogueCompleted = false;
    private int visibleCharIndex = 0;
    private Coroutine typingCoroutine = null;
    private WaitForSeconds delay;
    private WaitForSeconds skipDelay;
    private UI_Panel panel;

    #region Events
    public event Action OnDialogueCompleted;
    #endregion

    private void Start() {
        panel = dialoguePanel.GetComponent<UI_Panel>();
        delay = new WaitForSeconds(1.0f / typingSpeed);
        skipDelay = new WaitForSeconds(1.0f / (typingSpeed * skipSpeed));
        InputManager.Instance.InputActions.Dialogue.Disable();

        panel.OnPanelEnabled += OnPanelEnabled;
        panel.OnPanelDisabled += OnPanelDisabled;
        OnDialogueCompleted += OnDialogueComplete;
        InputManager.Instance.OnDialogueProceed += Skip;
    }

    private void OnDestroy() {
        panel.OnPanelEnabled -= OnPanelEnabled;
        panel.OnPanelDisabled -= OnPanelDisabled;
        OnDialogueCompleted -= OnDialogueComplete;
        InputManager.Instance.OnDialogueProceed -= Skip;
    }

    private void OnDialogueComplete() {
        isDialogueCompleted = true;
    }

    private void Skip() {
        if (!dialoguePanel.activeSelf)
            return;

        if (isDialogueCompleted && !isSkipping) {
            CloseDialogue();
            return;
        }

        isSkipping = true;
        StopCoroutine(typingCoroutine);
        dialogueTxt.maxVisibleCharacters = dialogueTxt.textInfo.characterCount;
        OnDialogueCompleted?.Invoke();
        isSkipping = false;
    }

    private void ResetDialogue() {
        isSkipping = false;
        isDialogueCompleted = false;

        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }

        dialogueTxt.maxVisibleCharacters = 0;
        visibleCharIndex = 0;
    }

    public void ShowDialogue(string text) {
        SetText(text);
        dialoguePanel.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText());
    }

    [Button]
    private void CloseDialogue() {
        ResetDialogue();
        dialoguePanel.SetActive(false);
    }

    private void OnPanelEnabled() {
        InputManager.Instance.InputActions.Game.Disable();
        InputManager.Instance.InputActions.Dialogue.Enable();
    }

    private void OnPanelDisabled() {
        InputManager.Instance.InputActions.Game.Enable();
        InputManager.Instance.InputActions.Dialogue.Disable();
    }

    public void SetText(string text) {
        ResetDialogue();
        tempTxt = text;
        dialogueTxt.text = tempTxt;
    }

    private IEnumerator TypeText() {
        TMP_TextInfo txtInfo = dialogueTxt.textInfo;

        while (visibleCharIndex < (txtInfo.characterCount + 1)) {
            int lastCharIndex = txtInfo.characterCount - 1;

            if (visibleCharIndex >= lastCharIndex) {
                dialogueTxt.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.25f);
                OnDialogueCompleted?.Invoke();
                yield break;
            }

            dialogueTxt.maxVisibleCharacters++;
            yield return isSkipping ? skipDelay : delay;
            visibleCharIndex++;
        }
    }
} 
