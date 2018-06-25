using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBottomMenu : MonoBehaviour {
    public GameMenu _GameMenu;
    public Button _ButtonMenu;
    public UIButton _ButtonUndo;

    void Start () {
        _ButtonMenu.onClick.AddListener(onClickMenu);
        _ButtonUndo.Btn.onClick.AddListener(onClickUndo);
    }

    void onClickMenu() {
        if (_GameMenu.IsShow) {
            _GameMenu.hide();
        } else {
            _GameMenu.show();
        }
    }

    void onClickUndo() {
        MoveCardMgr.Instance.undo();
    }

    public void show(System.Action cbEnd = null) {
        _cbShowEnd = cbEnd;

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", new Vector3(0, -90, 0),
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

    RectTransform RectTrans { get { return GetComponent<RectTransform>(); } }
}
