using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType { BAG, WEAPON, ARMOR, ACTION }

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public SlotType slotType;
    public ItemUI itemUI;

    private void OnDisable() {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }

#region 实现接口
    public void OnPointerEnter(PointerEventData eventData) {
        if (itemUI.GetItem()) {
            InventoryManager.Instance.toolTip.SetUpToolTip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount % 2 == 0) {
            UseItem();
        }
    }
#endregion

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
