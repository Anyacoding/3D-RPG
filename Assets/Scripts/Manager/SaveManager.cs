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
        Debug.Log("玩家数据已保存!");
    }

    public void LoadPlayerData() {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
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
