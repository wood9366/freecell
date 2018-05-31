using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseSplash : MonoBehaviour {
    public float _TimeBlack = 0.5f;
    public float _TimeFadeIn = 1;
    public float _TimeDisplay = 1;
    public float _TimeFadeOut = 1;

    void Start() {
        SpriteLogo.gameObject.SetActive(true);

        var color = SpriteLogo.color;
        color.a = 0.0f;
        SpriteLogo.material.SetColor("_Color", color);

        Timer.Instance.setTimeOut(_TimeBlack, onBlackComplete);
    }

    void onBlackComplete() {
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 1,
                                              "time", _TimeFadeIn,
                                              "oncomplete", "onFadeInComplete"));
    }

    void onFadeInComplete() {
        Timer.Instance.setTimeOut(_TimeDisplay, onDisplayComplete);
    }

    void onDisplayComplete() {
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 0,
                                              "time", _TimeFadeOut,
                                              "oncomplete", "onFadeOutComplete"));
    }

    void onFadeOutComplete() {
        SceneManager.LoadScene("menu");
    }

    SpriteRenderer SpriteLogo {
        get {
            if (_spriteLogo == null) {
                _spriteLogo = GetComponent<SpriteRenderer>();
            }

            return _spriteLogo;
        }
    }

    SpriteRenderer _spriteLogo = null;
}
