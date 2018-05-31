using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Button _ButtonStart;

	void Start () {
        playState("in");
        _ButtonStart.onClick.AddListener(onClickStart);
	}

    void onClickStart() {
        if (!_isStart) return;

        playState("out");
    }

    void Update() {
        var state = Anim.GetCurrentAnimatorStateInfo(0);

        if (isStateEnd(state, "in")) {
            _isStart = true;
        } else if (isStateEnd(state, "out")) {
            SceneManager.LoadScene("game");
        }
    }

    void playState(string name) {
        Anim.Play(name);
    }

    bool isStateEnd(string name) {
        var state = Anim.GetCurrentAnimatorStateInfo(0);

        return isStateEnd(state, name);
    }

    bool isStateEnd(AnimatorStateInfo state, string name) {
        return state.IsName(name) && state.normalizedTime >= 1;
    }

    Animator Anim {
        get {
            return GetComponent<Animator>();
        }
    }

    bool _isStart = false;
}
