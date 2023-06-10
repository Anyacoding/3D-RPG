using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    public Image icon = null;
    public Text amount = null;

    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;

    public void SetUpItemUI(InventoryItem inventoryItem) {
        if (inventoryItem.amount == 0) {
            Bag.items[Index].itemData = null;
        }

        if (inventoryItem.itemData != null) {
            icon.sprite = inventoryItem.itemData.itemIcon;
            amount.text = inventoryItem.amount.ToString();
            icon.gameObject.SetActive(true);
        }
        else {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem() {
        return Bag.items[Index].itemData;
    }
}
