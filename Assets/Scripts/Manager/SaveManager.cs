using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager> {
    string sceneName = "anya";

    public string SceneName {
        get => PlayerPrefs.GetString(sceneName);
    }

    #region 生命周期函数
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().name == "Main") {
                Application.Quit();
                Debug.Log("退出游戏");
                return;
            }
            SaveManager.Instance.SavePlayerData();
            SceneController.Instance.TransitionToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadPlayerData();
        }
    }

#endregion

    public void SavePlayerData() {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
        // 保存背包数据
        Save(InventoryManager.Instance.bagData, InventoryManager.Instance.bagData.name);
        Save(InventoryManager.Instance.actionData, InventoryManager.Instance.actionData.name);
        Save(InventoryManager.Instance.equipmentData, InventoryManager.Instance.equipmentData.name);
        Debug.Log("玩家数据已保存!");
    }

    public void LoadPlayerData() {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
        // 加载背包数据并刷新
        // TODO: 不确定这样好不好
        Load(InventoryManager.Instance.bagData, InventoryManager.Instance.bagData.name);
        Load(InventoryManager.Instance.actionData, InventoryManager.Instance.actionData.name);
        Load(InventoryManager.Instance.equipmentData, InventoryManager.Instance.equipmentData.name);
        InventoryManager.Instance.bagUI.RefreshUI();
        InventoryManager.Instance.actionUI.RefreshUI();
        InventoryManager.Instance.equipmentUI.RefreshUI();
        Debug.Log("玩家数据已加载!");
    }

    public void Save(Object data, string key) {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key) {
        if (PlayerPrefs.HasKey(key)) {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

}
