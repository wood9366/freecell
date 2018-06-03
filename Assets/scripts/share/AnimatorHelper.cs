using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour {
    public System.Action<string> OnAnimationEnd;

    static public AnimatorHelper Get(GameObject obj) {
        var helper = obj.GetComponent<AnimatorHelper>();

        if (helper == null) {
            helper = obj.AddComponent<AnimatorHelper>();
        }

        return helper;
    }

    void Update() {
        var state = Anim.GetCurrentAnimatorStateInfo(0);

        if (isStateEnd(state, _stateName)) {
            if (OnAnimationEnd != null) {
                OnAnimationEnd(_stateName);
            }
        }
    }

    public void play(string name) {
        _stateName = name;
        Anim.Play(name);
    }

    string _stateName = "";

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
}
