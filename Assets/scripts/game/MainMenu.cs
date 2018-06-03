using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Button _ButtonStart;

	void Start () {
        _ButtonStart.onClick.AddListener(onClickStart);

        Anim.OnAnimationEnd += onAnimEnd;
        Anim.play("in");
	}

    void onClickStart() {
        if (!_isStart) return;

        Anim.play("out");
    }

    void onAnimEnd(string name) {
        if (name == "in") {
            _isStart = true;
        } else if (name == "out") {
            SceneManager.LoadScene("game");
        }
    }

    AnimatorHelper Anim {
        get {
            return AnimatorHelper.Get(gameObject);
        }
    }

    bool _isStart = false;
}
