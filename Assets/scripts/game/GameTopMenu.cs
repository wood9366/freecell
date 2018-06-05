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
            TextTime.text = string.Format("{0}", new TimeSpan((long)(time * 10000000)));
        }
    }
}
