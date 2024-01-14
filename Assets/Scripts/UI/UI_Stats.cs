using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Stats : MonoBehaviour {
    [SerializeField] InventorySO playerInventoryRef;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI dayTxt;
    [SerializeField] TextMeshProUGUI coinsTxt;

    private void Start() {
        if (playerInventoryRef != null) {
            playerInventoryRef.OnCurrencyChanged += UpdateCoins;
        }
        DayCycleManager.Instance.OnDayChanged += UpdateDay;

        UpdateDay();
        UpdateCoins();
    }

    private void UpdateDay() {
        dayTxt.text = DayCycleManager.Instance.CurrentDay.ToString();
    }

    private void UpdateCoins() {
        coinsTxt.text = playerInventoryRef.currency.ToString();
    }
}
