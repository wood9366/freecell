using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDrop : State {
    public GameStateDrop(Game game) {
        _game = game;
    }

    public override void enter() {
        dropCards();
    }

    void dropCards() {
        DebugUtility.Assert(_game.Cards.Count > 0);

        _numShuffleCardFly = _game.Cards.Count;

        foreach (var card in _game.Cards) {
            card.fly(card.transform.position,
                        _game._SendDeck.transform.position,
                        onDropCardEnd,
                        Random.Range(0, 0.2f), 0,
                        false, 20,
                        iTween.EaseType.linear);
        }
    }

    void onDropCardEnd() {
        if (--_numShuffleCardFly <= 0) {
            dropEnd();
        }
    }

    void dropEnd() {
        _game.changeStatus(Game.EStatus.READY);
    }

    int _numShuffleCardFly = 0;
    Game _game;
}
