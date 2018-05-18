using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wood9366 {

    public class Toggle : MonoBehaviour {
        public System.Action<bool> OnToggle;

        public Color[] _BackgroundColor = new Color[2] { Color.white, Color.white };
        public Color[] _TextColor = new Color[2] { Color.white, Color.white };

        void Awake() {
            setIsOn(true, true);
        }

        void Start() {
            EventListener2D.Get(gameObject).OnClick += onClick;
        }

        void onClick() { toggle(); }

        public void on() { setIsOn(true); }
        public void off() { setIsOn(false); }
        public void toggle() { setIsOn(!_isOn); }

        public bool IsOn {
            get { return _isOn; }
            set { setIsOn(value); }
        }

        void setIsOn(bool isOn, bool force = false) {
            if (force || _isOn != isOn) {
                _isOn = isOn;

                SpriteBG.color = _BackgroundColor[DataIdx];
                TextLabel.color = _TextColor[DataIdx];

                if (OnToggle != null) OnToggle(IsOn);
            }
        }

        int DataIdx { get { return IsOn ? 0 : 1; } }

        bool _isOn = false;

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
