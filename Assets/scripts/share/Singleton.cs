using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T: class, new() {
    static public T Instance {
        get {
            if (sInstance == null) {
                sInstance = new T();
            }

            return sInstance;
        }
    }

    static T sInstance = default(T);
}
