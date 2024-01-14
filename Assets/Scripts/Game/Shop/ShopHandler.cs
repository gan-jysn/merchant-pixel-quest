using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour {
    [SerializeField] InventorySO shopInventory;
    [SerializeField] InventoryHandler inventoryHandler;
    [SerializeField] UI_Shop shopUI;
    [SerializeField] Shopkeeper shopKeeper;
    [SerializeField] CameraFocus cameraFocusType;

    [Header("Position References")]
    [SerializeField] ShopTrigger interiorTrigger;
    [SerializeField] ShopTrigger exteriorTrigger;
    [SerializeField] Transform shopInteriorSpawn;
    [SerializeField] Transform shopExteriorSpawn;

    public int PlayerCurrency { get { return inventoryHandler.Currency; } }
    public int PlayerInventorySize { get { return inventoryHandler.Items.Count; } }
    public List<ItemSO> PlayerInventory { get { return inventoryHandler.Items; } }

    public int ShopCurrency { get { return shopInventory.Currency; } }
    public int ShopInventorySize { get { return shopInventory.Items.Count; } }
    public List<ItemSO> ShopInventory { get { return shopInventory.Items; } }

    private GameObject playerRef;

    #region Events
    public event Action OnShopMadeSale; //Trigger when Shop made a sale
    public event Action OnShopReceiveNewItem;   //Trigger when Player sold an Item
    #endregion

    private void Start() {
        if (inventoryHandler != null) {
            playerRef = inventoryHandler.gameObject;
        }

        AddEventCallbacks();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void AddEventCallbacks() {
        if (shopKeeper != null) {
            shopKeeper.OnInteract += OpenShop;
        }

        exteriorTrigger.OnPlayerEntered += EnterShop;
        interiorTrigger.OnPlayerEntered += ExitShop;
    }

    private void RemoveEventCallbacks() {
        if (shopKeeper != null) {
            shopKeeper.OnInteract -= OpenShop;
        }

        exteriorTrigger.OnPlayerEntered -= EnterShop;
        interiorTrigger.OnPlayerEntered -= ExitShop;
    }

    private void EnterShop() {
        StartCoroutine(DelayedAction(() => {
            if (playerRef != null) {
                playerRef.transform.position = shopInteriorSpawn.transform.position;
                //Add CameraHandler Call to Switch Active Virtual Camera via ShopType Enum
                CameraHandler.Instance.SetCameraFocus(cameraFocusType);
            }
        }));
    }

    private void ExitShop() {
        StartCoroutine(DelayedAction(() => {
            playerRef.transform.position = shopExteriorSpawn.transform.position;
            //Add CameraHandler Call to Switch Active Virtual Camera via ShopType Enum
            CameraHandler.Instance.SetCameraFocus(CameraFocus.PlayerFocus);
        }));
    }

    private IEnumerator DelayedAction(Action action) {
        InputManager.Instance.InputActions.Game.Disable();
        action?.Invoke();
        yield return new WaitForSeconds(0.5f);
        InputManager.Instance.InputActions.Game.Enable();
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
