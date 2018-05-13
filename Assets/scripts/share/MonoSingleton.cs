using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour {

    static public T Instance { get { return sInstance; } }
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
