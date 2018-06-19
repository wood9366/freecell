using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : State {

    public void setCurrentState(int id) {
        if (_states.ContainsKey(id)) {
            _curState = _states[id];
        }
    }

    public void addState(int id, State state) {
        _states.Add(id, state);
    }

    public override void update(float deltaTime) {
        if (_curState != null) {
            _curState.update(deltaTime);
        }
    }

    State _curState = null;
    Dictionary<int, State> _states = new Dictionary<int, State>();
}
