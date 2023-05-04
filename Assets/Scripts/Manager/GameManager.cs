using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [HideInInspector]
    public CharacterStats playerStats;
    List<IEndGameObserver> endGameObservers;

#region 生命周期
    protected override void Awake() {
        base.Awake();
        endGameObservers = new List<IEndGameObserver>();
    }

    void Update() {
        if (playerStats != null) {
            // 检测玩家的生命值，归零时进行广播
            if (playerStats.CurrentHealth == 0) {
                NotifyObservers();
            }
        }
    }

#endregion

#region 订阅函数
    public void RigisterPlayer(CharacterStats player) {
        playerStats = player;
    }

    public void AddObserver(IEndGameObserver observer) {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer) {
        endGameObservers.Remove(observer);
    }
#endregion


#region 广播
    public void NotifyObservers() {
        foreach(var observer in endGameObservers) {
            observer.EndNotify();
        }
    }
#endregion

}
