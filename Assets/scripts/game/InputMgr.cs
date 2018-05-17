using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoSingleton<InputMgr> {
    public System.Action<Vector2> OnDrag;
    public System.Action<Vector2> OnDragEnd;
    public System.Action<Vector2> OnTap;

    const float TAP_TIME_MAX = 0.2f;
    const float HOLD_TIME_MIN = 0.3f;

    void Update() {
        #if UNITY_EDITOR || UNITY_STANDALONE
        detectMouse();
        #elif UNITY_IOS || UNITY_ANDROID
        detectTouch();
        #endif
    }

    #region Mouse
    void detectMouse() {
        if (Input.GetMouseButtonDown(0)) {
            _touchBeginTime = Time.time;
            _mouseButtonDownPosition = Input.mousePosition;
            _isTouchDrag = false;
        }

        if (Input.GetMouseButton(0)) {
            if (Input.mousePosition == _mouseButtonDownPosition) {
                _isTouchDrag = IsHold;
            } else {
                _isTouchDrag = true;
            }

            if (_isTouchDrag) {
                if (OnDrag != null) OnDrag(MouseScreenPosition);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_isTouchDrag) {
                if (OnDragEnd != null) OnDragEnd(MouseScreenPosition);
            } else if (IsTap) {
                if (OnTap != null) OnTap(Input.mousePosition);
            }
        }
    }

    Vector2 MouseScreenPosition {
        get {
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    Vector3 _mouseButtonDownPosition = Vector3.zero;
    #endregion

    #region Touch
    void detectTouch() {
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);

            switch(touch.phase) {
                case TouchPhase.Began:
                    _touchBeginTime = Time.time;
                    _isTouchDrag = false;
                    break;
                case TouchPhase.Moved:
                    _isTouchDrag = true;
                    if (OnDrag != null) OnDrag(touch.position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (_isTouchDrag) {
                        if (OnDragEnd != null) OnDragEnd(touch.position);
                    } else if (IsTap) {
                        if (OnTap != null) OnTap(touch.position);
                    }
                    break;
                case TouchPhase.Stationary:
                    _isTouchDrag = IsHold;

                    if (_isTouchDrag) {
                        if (OnDrag != null) OnDrag(touch.position);
                    }
                    break;
            }
        }
    }
    #endregion

    bool IsTap { get { return Time.time - _touchBeginTime <= TAP_TIME_MAX; } }
    bool IsHold { get { return Time.time - _touchBeginTime > HOLD_TIME_MIN; } }

    float _touchBeginTime = 0;
    bool _isTouchDrag = false;

    // Card _pointCard = null;
    // bool _isDrag = false;
    // bool _isHold = false;
    // Collider2D[] _colliders = new Collider2D[1];
}
