using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType { BAG, WEAPON, ARMOR, ACTION }

public class SlotUI : MonoBehaviour, IPointerClickHandler {
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount % 2 == 0) {
            UseItem();
        }
    }

    public void UseItem() {
        if (itemUI.GetItem()?.itemType == ItemType.USEABLE && itemUI.Bag.items[itemUI.Index].amount > 0) {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableItemData.healthPoint);
            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItem();
    }

    public void UpdateItem() {
        switch (slotType) {
            case SlotType.BAG: {
                itemUI.Bag = InventoryManager.Instance.bagData;
                break;
            }
            case SlotType.WEAPON: {
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                // 装备武器，切换武器
                if (itemUI.GetItem() != null) {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.GetItem());
                }
                else {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break;
            }
            case SlotType.ARMOR: {
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            }
            case SlotType.ACTION: {
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
            }
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item);
    }
}
