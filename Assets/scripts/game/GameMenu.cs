using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {
    public UIButton _ButtonNewRound;
    public UIButton _ButtonRestart;
    public UIToggle _ToggleAuto;

    public float _MenuSlideAnimTime = 0.2f;
    public iTween.EaseType _MenuSlideInAnimEaseType = iTween.EaseType.easeInExpo;
    public iTween.EaseType _MenuSlideOutAnimEaseType = iTween.EaseType.easeOutExpo;

	void Start () {
        _ToggleAuto.OnToggle += onToggleAuto;
	}

    public bool IsShow { get { return _isDisplay; } }

    public void show() {
        if (_isMoving) return;

        _isMoving = true;
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", RectTrans.anchoredPosition.x,
                                   "to", -397,
                                   "time", _MenuSlideAnimTime,
                                   "easetype", _MenuSlideInAnimEaseType,
                                   "onupdate", "onUpdateMenuPos",
                                   "oncomplete", "onShowEnd"));
    }

    public void hide() {
        if (_isMoving) return;

        _isMoving = true;
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", RectTrans.anchoredPosition.x,
                                   "to", 0,
                                   "time", _MenuSlideAnimTime,
                                   "easetype", _MenuSlideOutAnimEaseType,
                                   "onupdate", "onUpdateMenuPos",
                                   "oncomplete", "onHideEnd"));
    }

    void onUpdateMenuPos(float val) {
        var pos = RectTrans.anchoredPosition;

        pos.x = val;
        RectTrans.anchoredPosition = pos;
    }

    void onShowEnd() {
        _isMoving = false;
        _isDisplay = true;
    }

    void onHideEnd() {
        _isMoving = false;
        _isDisplay = false;
    }

    public void onClickNewRound() {
        if (!_isDisplay) return;

        Game.Instance.newRound();
        hide();
    }

    public void onClickRestart() {
        if (!_isDisplay) return;

        Game.Instance.restart();
        hide();
    }

    public void onClickMainMenu() {
        if (!_isDisplay) return;

        SceneManager.LoadScene("menu");
    }

    void onToggleAuto(bool isOn) {
        Game.Instance.IsAutoPutCardToFinal = isOn;
    }

    bool _isMoving = false;
    bool _isDisplay = false;

    RectTransform RectTrans { get { return GetComponent<RectTransform>(); } }
}
