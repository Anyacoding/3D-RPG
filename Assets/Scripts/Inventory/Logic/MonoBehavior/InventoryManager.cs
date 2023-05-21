using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager> {
    // TODO: 将来改成template data来实现保存和加载
    [Header("InventoryData")]
    public InventoryData_SO bagData;

    [Header("Container")]
    public ContainerUI inventoryContainer;
    

#region 生命周期函数
    void Start() {
        inventoryContainer.RefreshUI();
    }

#endregion

}
