using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTopMenu : MonoBehaviour {
    public Text TextTimeLabel;
    public Text TextTime;
    public UISign SignAuto;

    public void show(System.Action cbEnd = null) {
        _cbShowEnd = cbEnd;

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", new Vector3(0, 80, 0),
                                   "to", Vector3.zero,
                                   "time", 0.5f,
                                   "easetype", iTween.EaseType.easeOutCubic,
                                   "onupdate", "onUpdateMenuPos",
                                   "oncomplete", "onShowEnd"));
    }

    void onUpdateMenuPos(Vector3 val) {
        RectTrans.anchoredPosition = val;
    }

    void onShowEnd() {
        if (_cbShowEnd != null) {
            _cbShowEnd();
        }
    }

    System.Action _cbShowEnd;

    public void hideRoundTime() {
        TextTimeLabel.gameObject.SetActive(false);
        TextTime.gameObject.SetActive(false);
    }

    public void setRoundTime(int time) {
        TextTimeLabel.gameObject.SetActive(time >= 0);
        TextTime.gameObject.SetActive(time >= 0);

        if (time >= 0) {
            int second = time % 60;
            int minute = time / 60;
            int hour = minute / 60;

            minute %= 60;

            TextTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }
    }

    RectTransform RectTrans { get { return GetComponent<RectTransform>(); } }
}
