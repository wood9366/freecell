using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem2D : Singleton<EventSystem2D> {
    public System.Action<Vector3, Collider2D> OnDragStart;
    public System.Action<Vector3, Collider2D> OnDrag;
    public System.Action<Vector3, Collider2D> OnDragEnd;
    public System.Action<Vector3, Collider2D> OnClick;

    public void init() {
        InputMgr.Instance.OnDrag += onDrag;
        InputMgr.Instance.OnDragEnd += onDragEnd;
        InputMgr.Instance.OnTap += onTap;
    }

    public void release() {
        InputMgr.Instance.OnDrag -= onDrag;
        InputMgr.Instance.OnDragEnd -= onDragEnd;
        InputMgr.Instance.OnTap -= onTap;
    }

    void onDrag(Vector2 pos) {
        _touchPosition = pos;

        if (_dragCollider == null) {
            _dragCollider = getColliderOverlapPoint(TouchPositionWorld);

            if (_dragCollider != null) {
                if (OnDragStart != null) OnDragStart(TouchPositionWorld, _dragCollider);
                notifyEventListener(_dragCollider, "onDragStart");
            }
        }

        if (_dragCollider != null) {
            if (OnDrag != null) OnDrag(TouchPositionWorld, _dragCollider);
            notifyEventListener(_dragCollider, "onDrag");
        }
    }

    void onDragEnd(Vector2 pos) {
        _touchPosition = pos;

        if (_dragCollider != null) {
            if (OnDragEnd != null) OnDragEnd(TouchPositionWorld, _dragCollider);
            notifyEventListener(_dragCollider, "onDragEnd");
            _dragCollider = null;
        }
    }

    Collider2D _dragCollider = null;
    
    void onTap(Vector2 pos) {
        _touchPosition = pos;

        var collider = getColliderOverlapPoint(TouchPositionWorld);

        if (collider != null) {
            if (OnClick != null) OnClick(TouchPositionWorld, collider);
            notifyEventListener(collider, "onClick");
        }
    }

    void notifyEventListener(Collider2D collider, string evt) {
        var listener = collider.GetComponent<EventListener2D>();

        if (listener != null) {
            listener.notifyEvent(evt, TouchPositionWorld);
        }
    }

    Collider2D getColliderOverlapPoint(Vector3 pos) {
        int n = Physics2D.OverlapPointNonAlloc(pos,
                                               _colliders,
                                               Const.LAYER_MASK_UI);

        if (n > 0) {
            return _colliders[0];
        }

        return null;
    }

    Collider2D[] _colliders = new Collider2D[1];

    Vector3 TouchPositionWorld {
        get {
            return Camera.main.ScreenToWorldPoint(
                new Vector3(_touchPosition.x,
                            _touchPosition.y,
                            Camera.main.nearClipPlane));
        }
    }

    Vector2 _touchPosition = Vector2.zero;
}
