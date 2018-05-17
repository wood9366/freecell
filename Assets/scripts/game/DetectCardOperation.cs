using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCardOperation {
    public System.Action<Vector3, Card> OnDrag;
    public System.Action<Vector3, Card> OnDragEnd;
    public System.Action<Vector3, Card> OnTap;

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
        if (_dragCard == null) {
            _dragCard = getCardOverlapPoint(screenToWorldPos(pos));
        }

        if (_dragCard != null && OnDrag != null) {
            OnDrag(screenToWorldPos(pos), _dragCard);
        }
    }

    void onDragEnd(Vector2 pos) {
        if (_dragCard != null) {
            OnDragEnd(screenToWorldPos(pos), _dragCard);
            _dragCard = null;
        }
    }

    void onTap(Vector2 pos) {
        var worldPos = screenToWorldPos(pos);

        var card = getCardOverlapPoint(worldPos);

        if (card != null && OnTap != null) {
            OnTap(screenToWorldPos(worldPos), card);
        }
    }

    Card getCardOverlapPoint(Vector3 pos) {
        if (Physics2D.OverlapPointNonAlloc(pos, _colliders,
                                           Const.LAYER_MASK_UI) > 0)
        {
            return _colliders[0].GetComponent<Card>();
        }

        return null;
    }

    Vector3 screenToWorldPos(Vector2 screenPos) {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
    }

    Card _dragCard = null;
    Collider2D[] _colliders = new Collider2D[1];
}
