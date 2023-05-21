using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour {
    public List<SlotUI> slotHolders = new List<SlotUI>();

    public void RefreshUI() {
        for (int i = 0; i < slotHolders.Count; ++i) {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
