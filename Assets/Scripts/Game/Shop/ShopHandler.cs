using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour {
    [SerializeField] InventorySO shopInventory;
    [SerializeField] InventoryHandler inventoryHandler;
    [SerializeField] UI_Shop shopUI;
    [SerializeField] ShopTrigger trigger;

    public int PlayerCurrency { get { return inventoryHandler.Currency; } }
    public int PlayerInventorySize { get { return inventoryHandler.Items.Count; } }
    public List<ItemSO> PlayerInventory { get { return inventoryHandler.Items; } }

    public int ShopCurrency { get { return shopInventory.Currency; } }
    public int ShopInventorySize { get { return shopInventory.Items.Count; } }
    public List<ItemSO> ShopInventory { get { return shopInventory.Items; } }

    #region Events
    public event Action OnShopMadeSale; //Trigger when Shop made a sale
    public event Action OnShopReceiveNewItem;   //Trigger when Player sold an Item
    #endregion

    private void Start() {
        if (trigger == null) {
            trigger = FindObjectOfType<ShopTrigger>();
        }

        AddEventCallbacks();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void AddEventCallbacks() {
        if (trigger != null) {
            trigger.OnInteract += OpenShop;
        }
    }

    private void RemoveEventCallbacks() {
        if (trigger != null) {
            trigger.OnInteract -= OpenShop;
        }
    }

    public ItemSO GetShopItemViaID(int id) {
        return shopInventory.GetItemViaID(id);
    }

    public ItemSO GetPlayerItemViaID(int id) {
        return inventoryHandler.GetPlayerItemViaID(id);
    }

    public void OpenShop() {
        if (!shopUI.IsShopActive) {
            shopUI.OpenPanel();
            shopUI.ResetPanelUI();
        }
    }

    public void BuyItem() {
        ItemSO item = shopInventory.GetItemViaID(shopUI.ActiveBuyItemID);
        bool isPurchased = inventoryHandler.TryBuyItem(item);
        if (isPurchased) {
            shopInventory.AddCurrency(item.Value);
            shopInventory.RemoveItem(item);
            OnShopMadeSale?.Invoke();
        }
    }

    public void SellItem() {
        ItemSO item = inventoryHandler.GetPlayerItemViaID(shopUI.ActiveSellItemID);
        shopInventory.AddItem(item);
        inventoryHandler.SellItem(item);
        OnShopReceiveNewItem?.Invoke();
    }
}
