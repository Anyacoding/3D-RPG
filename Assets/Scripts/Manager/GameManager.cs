using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager> {
    [HideInInspector]
    public CharacterStats playerStats;
    List<IEndGameObserver> endGameObservers;

    private CinemachineFreeLook followCamera;

    #region 生命周期
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
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
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null) {
            followCamera.Follow = playerStats.transform;
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }
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

    public Transform GetEntrance() {
        foreach (var item in FindObjectsOfType<TransitionDestination>()) {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENETER || item.destinationTag == TransitionDestination.DestinationTag.ROOM) {
                return item.transform;
            }
        }
        return null;
    }

}
