using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener2D : MonoBehaviour {

    static public EventListener2D Get(GameObject obj) {
        var listener = obj.GetComponent<EventListener2D>();

        if (listener == null) {
            listener = obj.AddComponent<EventListener2D>();
        }

        return listener;
    }

    public System.Action<Vector3> OnDragStart;
    public System.Action<Vector3> OnDrag;
    public System.Action<Vector3> OnDragEnd;
    public System.Action OnClick;

    void onDragStart(Vector3 pos) { if (OnDragStart != null) OnDragStart(pos); }
    void onDrag(Vector3 pos) { if (OnDrag != null) OnDrag(pos); }
    void onDragEnd(Vector3 pos) { if (OnDragEnd != null) OnDragEnd(pos); }
    void onClick(Vector3 pos) { if (OnClick != null) OnClick(); }
}
