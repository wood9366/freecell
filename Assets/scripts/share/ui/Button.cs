using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wood9366 {

    public class Button : MonoBehaviour {
        public enum EStatus {
            NORMAL = 0,
            DISABLE
        }

        public Color[] _BackgroundColor = new Color[2] { Color.white, Color.white };
        public Color[] _TextColor = new Color[2] { Color.white, Color.white };

        public System.Action OnClick;

        void Awake() {
            setStatus(EStatus.NORMAL);
        }

        void Start() {
            EventListener2D.Get(gameObject).OnClick += onClick;
        }

        public bool IsEnabled {
            get { return Status == EStatus.NORMAL; }
            set { setStatus(value ? EStatus.NORMAL : EStatus.DISABLE); }
        }

        void onClick() {
            if (IsEnabled && OnClick != null) OnClick();
        }

        EStatus Status { get { return _status; } }

        void setStatus(EStatus status, bool force = false) {
            if (force || status != _status) {
                _status = status;

                SpriteBG.color = _BackgroundColor[(int)Status];
                TextLabel.color = _TextColor[(int)Status];
            } 
        }

        EStatus _status = EStatus.NORMAL;

        SpriteRenderer SpriteBG {
            get {
                if (_spriteBG == null) {
                    _spriteBG = GetComponent<SpriteRenderer>();
                }
                return _spriteBG;
            }
        }

        SpriteRenderer _spriteBG = null;

        TextMesh TextLabel {
            get {
                if (_textLabel == null) {
                    _textLabel = GetComponentInChildren<TextMesh>();
                }

                return _textLabel;
            }
        }

        TextMesh _textLabel = null;
    }

}
