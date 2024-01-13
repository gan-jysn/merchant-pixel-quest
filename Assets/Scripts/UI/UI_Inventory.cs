using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UI_Inventory : UI_Popup {
    [Header("Inventory References")]
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform itemViewContainer;
    [SerializeField] InventoryHandler inventoryHandler;

    [Header("Button References")]
    [SerializeField] Button useBtn;
    [SerializeField] TextMeshProUGUI useBtnTxt;

    [Header("Item Info References")]
    [SerializeField] TextMeshProUGUI itemNameTxt;
    [SerializeField] TextMeshProUGUI itemDescTxt;

    private int activeItemID;
    private List<UI_Item> UIItems = new List<UI_Item>();

    public override void Start() {
        base.Start();

        OnPopupOpened += OnPopupOpen;
        inventoryHandler.OnItemStored += UpdateInventory;
        ClearItemInfo();
        InitializeInventory();
    }

    private void OnDestroy() {
        OnPopupOpened -= OnPopupOpen;
        inventoryHandler.OnItemStored -= UpdateInventory;
    }

    private void OnPopupOpen() {
        UpdateInventory();
    }

    public void ClearItemInfo() {
        itemNameTxt.text = "";
        itemDescTxt.text = "";
    }

    private void InitializeInventory() {
        if (inventoryHandler == null) {
            Debug.Log("Unable to Initialize Inventory");
            return;
        }

        int inventorySize = inventoryHandler.Items.Count;
        for (int i = 0;i < inventorySize;i++) {
            CreateItemPrefab(inventoryHandler.Items[i]);
        }
    }

    [Button]
    public void UpdateInventory() {
        int inventorySize = inventoryHandler.Items.Count;

        //Update Data
        for (int i = 0;i < inventorySize;i++) {
            ItemSO data = inventoryHandler.Items[i];
            if ((i + 1) > UIItems.Count) {
                CreateItemPrefab(data);
            } else {
                UI_Item ui = UIItems[i];
                ui.gameObject.SetActive(true);
                ui.SetItemData(data);
                ui.UpdateUI();
            }
        }

        //Check if there are left over UI Item GameObjects
        if (UIItems.Count > inventorySize) {
            int excessObjCount = UIItems.Count - inventorySize;

            for (int i = inventorySize - 1;i < UIItems.Count;i++) {
                UIItems[i].gameObject.SetActive(false);
            }
        }

        //Select First Item
        if (UIItems != null && UIItems.Count > 0) {
            UIItems[0].Select();
        }
    }

    private void CreateItemPrefab(ItemSO itemData) {
        GameObject obj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, itemViewContainer);
        obj.SetActive(true);
        UI_Item item = obj.GetComponent<UI_Item>();
        UIItems.Add(item);
        item.SetItemData(itemData);
        item.UpdateUI();
        //Subscribe to event OnUIItemClicked to SetItemInfo
        item.OnItemSelected += SetItemInfo;
    }

    private UI_Item GetActiveItemData() {
        foreach (UI_Item item in UIItems) {
            if (item.ItemData.ItemID == activeItemID) {
                return item;
            }
        }

        Debug.Log("Unable to find ItemData");
        return null;
    }

    public void SetItemInfo(int itemID, ItemType type, string itemName, string itemDesc) {
        activeItemID = itemID;
        SetUseButton(type);
        itemNameTxt.text = itemName;
        itemDescTxt.text = itemDesc;
    }

    private void SetUseButton(ItemType type) {
        switch (type) {
            case ItemType.Clothing:
                useBtn.gameObject.SetActive(true);
                useBtnTxt.text = "Equip Item";
                break;
            case ItemType.Consumable:
                useBtn.gameObject.SetActive(true);
                useBtnTxt.text = "Use Item";
                break;
            case ItemType.Trophy:
                useBtn.gameObject.SetActive(false);
                break;
        }
    }

    public void EquipItem() {
        ItemSO item = inventoryHandler.GetPlayerItemViaID(activeItemID);
        inventoryHandler.EquiptItem(item);

        ClosePanel();
        useBtn.interactable = true;
    }
}
