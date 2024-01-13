using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : UI_Popup {
    [Header("Shop References")]
    [SerializeField] GameObject shopItemUIPrefab;
    [SerializeField] ShopHandler shopHandler;

    [Header("UI Elements - Panels")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject sellPanel;

    [Header("UI Elements - Buttons")]
    [SerializeField] Button buyBtn;
    [SerializeField] Button sellBtn;

    [Header("UI Elements - Buy Menu")]
    [SerializeField] Transform buyItemViewContainer;
    [SerializeField] TextMeshProUGUI buyItemNameTxt;
    [SerializeField] TextMeshProUGUI buyItemDescTxt;
    [SerializeField] TextMeshProUGUI buyPlayerCurrencyTxt;
    [SerializeField] List<UI_ShopItem> shopInvetoryUI = new List<UI_ShopItem>();

    [Header("UI Elements - Sell Menu")]
    [SerializeField] Transform sellItemViewContainer;
    [SerializeField] TextMeshProUGUI sellItemNameTxt;
    [SerializeField] TextMeshProUGUI sellItemDescTxt;
    [SerializeField] TextMeshProUGUI sellItemValueTxt;
    [SerializeField] List<UI_ShopItem> playerInvetoryUI = new List<UI_ShopItem>();

    private int activeBuyItemID;
    private int activeSellItemID;
    private bool isShopActive = false;

    public int ActiveBuyItemID { get { return activeBuyItemID; } }
    public int ActiveSellItemID { get { return activeSellItemID; } }
    public bool IsShopActive { get { return isShopActive; } }

    #region Events
    public event Action OnBuyPanelOpened;
    public event Action OnSellPanelOpened;
    #endregion

    public override void Start() {
        base.Start();
        AddEventCallbacks();
        ResetPanelUI();
        ClearItemInfo();
        InitializeInvetories();
    }

    private void OnDestroy() {
        RemoveEventCallbacks();
    }

    private void AddEventCallbacks() {
        OnBuyPanelOpened += UpdateShopInventory;
        OnSellPanelOpened += UpdatePlayerInventory;
        shopHandler.OnShopMadeSale += UpdateShopInventory;
        shopHandler.OnShopReceiveNewItem += UpdatePlayerInventory;
    }

    private void RemoveEventCallbacks() {
        OnBuyPanelOpened -= UpdateShopInventory;
        OnSellPanelOpened -= UpdatePlayerInventory;
        shopHandler.OnShopMadeSale -= UpdateShopInventory;
        shopHandler.OnShopReceiveNewItem -= UpdatePlayerInventory;
    }

    private void InitializeInvetories() {
        if (shopHandler == null) {
            Debug.Log("Unable to Initialize Inventories");
            return;
        }

        int shopInventorySize = shopHandler.ShopInventorySize;
        for (int i = 0;i < shopInventorySize;i++) {
            CreateItemPrefab(true, shopHandler.ShopInventory[i]);
        }

        int playerInventorySize = shopHandler.PlayerInventorySize;
        for (int i = 0;i < playerInventorySize;i++) {
            CreateItemPrefab(false, shopHandler.PlayerInventory[i]);
        }
    }

    private void UpdateShopInventory() {
        int shopInventorySize = shopHandler.ShopInventorySize;

        //Update Data
        for (int i = 0;i < shopInventorySize;i++) {
            ItemSO data = shopHandler.ShopInventory[i];
            if ((i + 1) > shopInvetoryUI.Count) {
                CreateItemPrefab(true, data);
            } else {
                UI_ShopItem shopItem = shopInvetoryUI[i];
                shopItem.gameObject.SetActive(true);
                shopItem.SetItemData(data);
                shopItem.UpdateUI();
            }
        }

        //Check Leftover UI GameObjects
        if (shopInvetoryUI.Count > shopInventorySize) {
            for (int i = shopInventorySize - 1;i < shopInvetoryUI.Count;i++) {
                shopInvetoryUI[i].gameObject.SetActive(false);
            }
        }

        //First Item Selection
        if (shopInvetoryUI != null && shopInvetoryUI.Count > 0) {
            shopInvetoryUI[0].Select();
        }
    }

    private void UpdatePlayerInventory() {
        int playerInventorySize = shopHandler.PlayerInventorySize;

        //Update Data
        for (int i = 0;i < playerInventorySize;i++) {
            ItemSO data = shopHandler.PlayerInventory[i];
            if ((i + 1) > playerInvetoryUI.Count) {
                CreateItemPrefab(false, data);
            } else {
                UI_ShopItem shopItem = playerInvetoryUI[i];
                shopItem.gameObject.SetActive(true);
                shopItem.SetItemData(data);
                shopItem.UpdateUI();
            }
        }

        //Check Leftover UI GameObjects
        if (playerInvetoryUI.Count > playerInventorySize) {
            for (int i = playerInventorySize - 1;i < playerInvetoryUI.Count;i++) {
                playerInvetoryUI[i].gameObject.SetActive(false);
            }
        }

        //First Item Selection
        if (playerInvetoryUI != null && playerInvetoryUI.Count > 0) {
            playerInvetoryUI[0].Select();
        }
    }

    private void CreateItemPrefab(bool isShopInvetory, ItemSO data) {
        Transform parent = isShopInvetory ? buyItemViewContainer : sellItemViewContainer;
        GameObject obj = Instantiate(shopItemUIPrefab, Vector3.zero, Quaternion.identity, parent);
        obj.SetActive(true);
        UI_ShopItem item = obj.GetComponent<UI_ShopItem>();
        item.SetItemData(data);
        item.UpdateUI();

        //Subscribe to event OnUIItemClicked to SetItemInfo
        if (isShopInvetory) {
            shopInvetoryUI.Add(item);
            item.OnItemSelected += SetBuyItemInfo;
        } else if (!isShopInvetory) {
            playerInvetoryUI.Add(item);
            item.OnItemSelected += SetSellItemInfo;
        }
    }

    public void SetBuyItemInfo(int itemID, int value, string itemName, string itemDesc) {
        activeBuyItemID = itemID;
        buyItemNameTxt.text = itemName;
        buyItemDescTxt.text = itemDesc;
        buyPlayerCurrencyTxt.text = shopHandler.PlayerCurrency.ToString();
    }

    public void SetSellItemInfo(int itemID, int value, string itemName, string itemDesc) {
        activeSellItemID = itemID;
        sellItemNameTxt.text = itemName;
        sellItemDescTxt.text = itemDesc;
        sellItemValueTxt.text = shopHandler.GetPlayerItemViaID(itemID).Value.ToString();
    }

    public void ClearItemInfo() {
        buyItemNameTxt.text = "";
        buyItemDescTxt.text = "";
        buyPlayerCurrencyTxt.text = "";

        sellItemNameTxt.text = "";
        sellItemDescTxt.text = "";
        sellItemValueTxt.text = "";
    }

    public void ResetPanelUI() {
        menuPanel.SetActive(true);
        buyPanel.SetActive(false);
        sellPanel.SetActive(false);
    }

    public override void OpenPanel() {
        base.OpenPanel();
        isShopActive = true;
    }

    public override void ClosePanel() {
        base.ClosePanel();
        isShopActive = false;
    }

    public void OpenBuyPanel() {
        SoundManager.Instance.PlayBtnSFX();
        OnBuyPanelOpened?.Invoke();
        StartCoroutine(DelayAction(() => {
            menuPanel.SetActive(false);
            buyPanel.SetActive(true);
        }));
    }

    public void OpenSellPanel() {
        SoundManager.Instance.PlayBtnSFX();
        OnSellPanelOpened?.Invoke();
        StartCoroutine(DelayAction(() => {
            menuPanel.SetActive(false);
            sellPanel.SetActive(true);
        }));
    }

    public void ReturnToMenu() {
        SoundManager.Instance.PlayBtnSFX();
        StartCoroutine(DelayAction(() => {
            buyPanel.SetActive(false);
            sellPanel.SetActive(false);
            menuPanel.SetActive(true);

            buyBtn.interactable = true;
            sellBtn.interactable = true;
        }));
    }

    public void BuyActiveItem() {
        shopHandler.BuyItem();
        SoundManager.Instance.PlayBtnSFX();
        StartCoroutine(DelayAction(() => {
            buyBtn.interactable = true;
            ClosePanel();
        }));
    }

    public void SellActiveItem() {
        shopHandler.SellItem();
        SoundManager.Instance.PlayBtnSFX();
        StartCoroutine(DelayAction(() => {
            sellBtn.interactable = true;
            ClosePanel();
        }));

    }

    private IEnumerator DelayAction(Action action) {
        yield return new WaitForSeconds(DelayTime);
        action?.Invoke();
    }
}
