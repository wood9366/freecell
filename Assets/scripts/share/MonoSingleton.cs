using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour {

    static public T Instance {
        get {
            if (sInstance == null) {
                var obj = new GameObject("_" + typeof(T).FullName);

                sInstance = obj.AddComponent<T>();
            }

            return sInstance;
        }
    }

    static T sInstance;

    protected virtual void load() { }
    protected virtual void init() { }
    protected virtual void release() { }

    void Awake() {
        sInstance = GetComponent<T>();

        load();
    }

    void Start() {
        init();
    }

    void OnDestroy() {
        release();
    }
}
