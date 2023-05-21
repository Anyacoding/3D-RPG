using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject {
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO itemData) {
        bool found = false;
        if (itemData.stackable) {
            foreach (var item in items) {
                if (item.itemData == itemData) {
                    item.amount += itemData.itemAmount;
                    found = true;
                    return;
                }
            }
        }
        for (int i = 0; i < items.Count; ++i) {
            if (items[i].itemData == null && !found) {
                items[i].itemData = itemData;
                items[i].amount = itemData.itemAmount;
                break;
            }
        }
    }
}

[System.Serializable]
public class InventoryItem {
    public ItemData_SO itemData;
    public int amount;
}
