using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {
    public ItemData_SO itemData;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // DONE: 将物品添加到背包
            InventoryManager.Instance.bagData.AddItem(itemData);
            InventoryManager.Instance.bagUI.RefreshUI();
            Destroy(gameObject);
        }
    }
}
