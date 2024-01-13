using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UI_Popup : MonoBehaviour {
    [SerializeField] GameObject popupPanel;
    [SerializeField] Button closeBtn;
    [SerializeField] float delayTime = 0.25f;
    [SerializeField] bool isInactiveStart = true;

    public float DelayTime { get { return delayTime; } }

    public bool IsPanelActive { get { return popupPanel.gameObject.activeSelf; } }

    #region Events
    public event Action OnPopupOpened;
    #endregion

    public virtual void Start() {
        //Set Default Active Status
        if (isInactiveStart) {
            popupPanel.SetActive(false);
        } else {
            popupPanel.SetActive(true);
        }

        if (closeBtn != null) {
            closeBtn.onClick.AddListener(() => { PlayBtnSFX(); });
        }

        popupPanel.GetComponent<UI_Panel>().OnPanelEnabled += OnPanelEnabled;
        popupPanel.GetComponent<UI_Panel>().OnPanelDisabled += OnPanelDisabled;
    }

    public virtual void OnPanelEnabled() {
        closeBtn.interactable = true;
    }

    public virtual void OnPanelDisabled() {

    }

    //Open Panel
    [Button]
    public virtual void OpenPanel() {
        popupPanel.SetActive(true);
        OnPopupOpened?.Invoke();
        GameManager.Instance.PauseGame();
    }

    //Close Panel
    [Button]
    public virtual void ClosePanel() {
        StartCoroutine(DelayClose());
    }

    //Adds Delay to Close for Button Animations & SFX
    private IEnumerator DelayClose() {
        closeBtn.interactable = false;
        popupPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
        yield return new WaitForSeconds(delayTime);
        closeBtn.interactable = true;
    }

    public void PlayBtnSFX() {
        SoundManager.Instance.PlayBtnSFX();
    }
}
