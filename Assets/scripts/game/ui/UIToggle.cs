using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour {
    public System.Action<bool> OnToggle;

    public Text _Label;
    public Image _On;
    public Image _Off;

    public void onClick() {
        IsOn = !IsOn;
    }

    public bool IsOn {
        get { return _isOn; }
        set {
            _isOn = value;

            iTween.ValueTo(_Label.gameObject,
                           iTween.Hash("from", _Label.GetComponent<RectTransform>().anchoredPosition.x,
                                       "to", _isOn ? 22 : -22,
                                       "time", 0.2f,
                                       "onupdatetarget", gameObject,
                                       "onupdate", "onTextUpdate"));

            iTween.ValueTo(_On.gameObject,
                           iTween.Hash("from", _Off.color.a,
                                       "to", _isOn ? 1 : 0,
                                       "time", 0.2f,
                                       "delay", 0.1f,
                                       "onupdatetarget", gameObject,
                                       "onupdate", "onImageOnUpdate"));

            iTween.ValueTo(_Off.gameObject,
                           iTween.Hash("from", _Off.color.a,
                                       "to", _isOn ? 0 : 1,
                                       "time", 0.2f,
                                       "delay", 0.1f,
                                       "onupdatetarget", gameObject,
                                       "onupdate", "onImageOffUpdate"));

            if (OnToggle != null) {
                OnToggle(_isOn);
            }
        }
    }

    void onTextUpdate(float val) {
        var pos = _Label.GetComponent<RectTransform>().anchoredPosition;

        pos.x = val;
        _Label.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    void onImageOnUpdate(float val) {
        updateImageColor(_On, val);
    }

    void onImageOffUpdate(float val) {
        updateImageColor(_Off, val);
    }

    void updateImageColor(Image image, float alpha) {
        var color = image.color;

        color.a = alpha;
        image.color = color;
    }

    bool _isOn = false;
}
