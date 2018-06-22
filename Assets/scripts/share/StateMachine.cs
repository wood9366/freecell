using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : State {

    public void setState(int id) {
        if (_states.ContainsKey(id)) {
            if (_states.ContainsKey(_curStateId)) {
                _curState.exit();
            }

            _curStateId = id;
            _curState = _states[id];

            _curState.enter();
        }
    }

    public void addState(int id, State state) {
        _states.Add(id, state);
    }

    public int CurrentStateId { get { return _curStateId; } }
    public State CurrentState { get { return _curState; } }

    public override void update(float deltaTime) {
        if (_curState != null) {
            _curState.update(deltaTime);
        }
    }

    int _curStateId = -1;
    State _curState = null;
    Dictionary<int, State> _states = new Dictionary<int, State>();
}
