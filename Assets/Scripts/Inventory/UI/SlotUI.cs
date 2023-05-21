using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType { BAG, WEAPON, ARMOR, ACTION }

public class SlotUI : MonoBehaviour {
    public SlotType slotType;
    public ItemUI itemUI;

    public void UpdateItem() {
        switch (slotType) {
            case SlotType.BAG: {
                itemUI.Bag = InventoryManager.Instance.bagData;
                break;
            }
            case SlotType.WEAPON: {
                break;
            }
            case SlotType.ACTION: {
                break;
            }
            case SlotType.ARMOR: {
                break;
            }
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item);
    }

}
