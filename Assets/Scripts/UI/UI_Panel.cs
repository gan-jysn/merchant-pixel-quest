using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel : MonoBehaviour {
    public event Action OnPanelEnabled;
    public event Action OnPanelDisabled;

    private void OnEnable() {
        OnPanelEnabled?.Invoke();
    }

    private void OnDisable() {
        OnPanelDisabled?.Invoke();
    }
}
