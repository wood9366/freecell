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
}
