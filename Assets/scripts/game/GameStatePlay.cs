using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlay : State {
    public GameStatePlay(Game game) {
        _game = game;
    }

    public override void enter() {
        _game._GameMenu._ButtonNewRound.Enabled = true;
        _game._GameMenu._ButtonRestart.Enabled = true;

        DeckDrag.Instance.autoMoveCardAndSwitchDeckCardToFinalDeck();

        RoundTime = 0;
        _roundTimerId = Timer.Instance.setInterval(1.0f, onRoundTimerUpdate);
    }

    void onRoundTimerUpdate() {
        _game._GameTopMenu.setRoundTime(++RoundTime);
    }

    public override void exit() {
        _game._GameMenu._ButtonRestart.Enabled = false;

        RoundTime = -1;
        Timer.Instance.clearTimeOut(_roundTimerId);
    }

    int RoundTime {
        get { return _roundTime; }
        set {
            _roundTime = value;
            _game._GameTopMenu.setRoundTime(_roundTime);
        }
    }

    int _roundTime = 0;
    int _roundTimerId = -1;
    Game _game;
}
