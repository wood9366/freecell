using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTopMenu : MonoBehaviour {
    public Text TextTimeLabel;
    public Text TextTime;
    public UISign SignAuto;

    public void setTime(int time) {
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
}
