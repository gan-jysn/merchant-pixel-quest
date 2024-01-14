using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory")]
public class InventorySO : ScriptableObject {
    public int currency;
    public List<ItemSO> Items = new List<ItemSO>();

    public int Currency { get { return currency; } protected set { } }

    #region Events
    public event Action OnCurrencyChanged;
    #endregion

    public void AddCurrency(int amountToAdd) {
        currency += amountToAdd;
        OnCurrencyChanged?.Invoke();
    }

    public bool TryDeductCurrency(int amountToDeduct) {
        int tempCurrency = currency - amountToDeduct;
        if (tempCurrency < 0) {
            Debug.Log("Insufficient Funds");
            return false;
        }

        return true;
    }

    public void DeductCurrency(int amountToDeduct) {
        if (TryDeductCurrency(amountToDeduct)) {
            currency -= amountToDeduct;
            OnCurrencyChanged?.Invoke();
        }
    }

    public void AddItem(ItemSO item) {
        Items.Add(item);
        Debug.Log(item.ItemName + " has been added to Inventory");
    }

    public void RemoveItem(ItemSO item) {
        Items.Remove(item);
        Debug.Log(item.ItemName + " has been removed from Inventory");
    }

    public ItemSO GetItemViaID(int id) {
        foreach (ItemSO item in Items) {
            if (item.ItemID == id) {
                return item;
            }
        }
        return null;
    }

    public int GetItemIndexFromInventory(ItemSO item) {
        int i = 0;
        Debug.Log(item.ItemID);
        Debug.Log(Items.Count);
        foreach (ItemSO itemSO in Items) {
            if (itemSO.ItemID == item.ItemID) {
                return i;
            } else {
                i++;
            }
        }
        return -1;
    }

    public int GetItemQuantityViaID(int id) {
        int quantity = 0;
        foreach (ItemSO item in Items) {
            if (item.ItemID == id) {
                quantity++;
            }
        }

        return quantity;
    }

    public int GetItemIndexFromInventoryViaID(int id) {
        ItemSO item = GetItemFromInventoryViaID(id);
        if (item != null) {
            int index = GetItemIndexFromInventory(item);
            return index;
        }

        return -1;
    }

    public ItemSO GetItemFromInventoryViaID(int id) {
        foreach (ItemSO item in Items) {
            if (item.ItemID == id) {
                return item;
            }
        }

        Debug.Log("Item not in Inventory");
        return null;
    }
}
