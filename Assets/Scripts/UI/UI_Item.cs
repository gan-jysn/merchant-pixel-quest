using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Item : MonoBehaviour, IPointerClickHandler {
    [SerializeField] ItemSO itemData;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI quantityTxt;

    public ItemSO ItemData { get { return itemData; } }
    #region Events
    public event Action<int, ItemType, string, string> OnItemSelected;
    #endregion

    private void OnEnable() {
        UpdateUI();
    }

    public void UpdateUI() {
        if (itemData != null) {
            iconImage.sprite = itemData.Icon;
            if (itemData.Type == ItemType.Consumable) {
                quantityTxt.text = "";  //Assign Quantity Value for Consumables
            } else {
                quantityTxt.text = "";
            }
        }
    }

    public void SetItemData(ItemSO data) {
        itemData = data;
    }

    public void Select() {
        SoundManager.Instance.PlayBtnSFX();
        OnItemSelected?.Invoke(itemData.ItemID, itemData.Type, itemData.ItemName, itemData.Description);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Select();
    }
}
