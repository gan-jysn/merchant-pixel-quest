using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_ShopItem : MonoBehaviour, IPointerClickHandler {
    [SerializeField] ItemSO itemData;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI itemNameTxt;
    [SerializeField] TextMeshProUGUI itemValueTxt;

    public ItemSO ItemData { get { return itemData; } }

    #region Events
    public event Action<int, int, string, string> OnItemSelected;
    #endregion

    private void OnEnable() {
        UpdateUI();
    }

    public void UpdateUI() {
        iconImage.sprite = itemData.Icon;
        itemNameTxt.text = itemData.ItemName;
        itemValueTxt.text = itemData.Value.ToString();
    }

    public void SetItemData(ItemSO data) {
        itemData = data;
    }

    public void Select() {
        SoundManager.Instance.PlayBtnSFX();
        OnItemSelected?.Invoke(itemData.ItemID, itemData.Value, itemData.ItemName, itemData.Description);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Select();
    }
}
