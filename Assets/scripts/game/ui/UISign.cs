using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISign : MonoBehaviour {

    public void show() {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", RectTrans.anchoredPosition.y,
                                   "to", -57,
                                   "time", 0.3f,
                                   "onupdate", "onUpdateSign"));
    }

    public void hide() {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", RectTrans.anchoredPosition.y,
                                   "to", 0,
                                   "time", 0.3f,
                                   "onupdate", "onUpdateSign"));
    }

    void onUpdateSign(float val) {
        var pos = RectTrans.anchoredPosition;

        pos.y = val;
        RectTrans.anchoredPosition = pos;
    }

    RectTransform RectTrans { get { return GetComponent<RectTransform>(); } }
}
