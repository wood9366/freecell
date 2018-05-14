using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEventListener : MonoBehaviour {

    static public MonoEventListener Get(GameObject obj) {
        var listener = obj.GetComponent<MonoEventListener>();

        if (listener == null) {
            listener = obj.AddComponent<MonoEventListener>();
        }

        return listener;
    }

    public System.Action OnMouseClick;

	void OnMouseDown() {
        if (OnMouseClick != null) {
            OnMouseClick();
        }
	}
}
