using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour {
    public KeyCode actionKey;
    private SlotUI currentSlotUI;

    private void Awake() {
        currentSlotUI = GetComponent<SlotUI>();
    }

    private void Update() {
        if (Input.GetKeyDown(actionKey) && currentSlotUI.itemUI.GetItem()) {
            currentSlotUI.UseItem();
        }
    }
}
