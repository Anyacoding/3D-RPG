using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private ItemUI currentItemUI;
    private SlotUI currentSlotUI;
    private SlotUI targetSlotUI;

    private void Awake() {
        currentItemUI = GetComponent<ItemUI>();
        currentSlotUI = GetComponentInParent<SlotUI>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // 记录原始数据
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalSlotUI = currentSlotUI;
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData) {
        // 跟随鼠标位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // 放下物品，交换数据
        if (EventSystem.current.IsPointerOverGameObject()) {
            if (CheckInContainerUI(eventData.position)) {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotUI>()) {
                    targetSlotUI = eventData.pointerEnter.gameObject.GetComponent<SlotUI>();
                }
                else {
                    targetSlotUI = eventData.pointerEnter.gameObject.GetComponentInParent<SlotUI>();
                }

                switch(targetSlotUI.slotType) {
                    case SlotType.BAG: {
                        // DONE: 有bug，可以通过交互蘑菇使得剑进入action bar
                        if (currentSlotUI.slotType == SlotType.WEAPON && (targetSlotUI.itemUI.GetItem() == null || targetSlotUI.itemUI.GetItem().itemType == ItemType.WEAPON))
                            SwapItem();
                        else if (currentSlotUI.slotType == SlotType.ACTION && (targetSlotUI.itemUI.GetItem() == null || targetSlotUI.itemUI.GetItem().itemType == ItemType.USEABLE))
                            SwapItem();  
                        else if (currentSlotUI.slotType == SlotType.BAG)
                            SwapItem();  
                        break;
                    }
                    case SlotType.WEAPON: {
                        if (currentItemUI.GetItem().itemType == ItemType.WEAPON)
                            SwapItem();
                        break;
                    }
                    case SlotType.ACTION: {
                        if (currentItemUI.GetItem().itemType == ItemType.USEABLE)
                            SwapItem();
                        break;
                    }
                    case SlotType.ARMOR: {
                        if (currentItemUI.GetItem().itemType == ItemType.ARMOR)
                            SwapItem();
                        break;
                    }
                }
                // 更新slot的UI
                currentSlotUI.UpdateItem();
                targetSlotUI.UpdateItem();
            }
        }
        // 设置回原来的父级
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        // 重新设置RectTransform位置
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    private bool CheckInContainerUI(Vector3 position) {
        return InventoryManager.Instance.CheckInventoryUI(position) ||
               InventoryManager.Instance.CheckActionUI(position)    ||
               InventoryManager.Instance.CheckEquipmentUI(position);
    }

    public void SwapItem() {
        if (targetSlotUI == currentSlotUI) return;

        var targetItem = targetSlotUI.itemUI.Bag.items[targetSlotUI.itemUI.Index];
        var tempItem = currentSlotUI.itemUI.Bag.items[currentSlotUI.itemUI.Index];
        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if (isSameItem && targetItem.itemData.stackable) {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else {
            currentSlotUI.itemUI.Bag.items[currentSlotUI.itemUI.Index] = targetItem;
            targetSlotUI.itemUI.Bag.items[targetSlotUI.itemUI.Index] = tempItem;
        }
    }
}
