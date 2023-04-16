using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance {
        get => instance;
    }

    public static bool IsInitialized {
        get => instance != null;
    }

#region 生命周期函数
    protected virtual void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }
    
#endregion
    
}
