using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePrepare : State {
    public GameStatePrepare(Game game) {
        _game = game;
    }

    public override void enter() {
        showRoundStartAnimation();
    }

    void showRoundStartAnimation() {
        System.Action<GameObject> fnHide = (go) => {
            foreach(var sp in go.GetComponentsInChildren<SpriteRenderer>()) {
                var c = sp.color;
                c.a = 0;
                sp.color =  c;
            }

            foreach(var m in go.GetComponentsInChildren<TextMesh>()) {
                var c = m.color;
                c.a = 0;
                m.color =  c;
            }
        };

        _game._DeckFinals.ForEach(deck => fnHide(deck.gameObject));
        _game._DeckSwitches.ForEach(deck => fnHide(deck.gameObject));
        _game._DeckCards.ForEach(deck => fnHide(deck.gameObject));

        _game._GameTopMenu.show(() => {
            int n = _game._DeckFinals.Count +
                _game._DeckSwitches.Count +
                _game._DeckCards.Count;

            System.Action fnCheckEnd = () => {
                n--;

                if (n <= 0) {
                    onRoundStartAnimationEnd();
                }
            };

            _game._DeckFinals.ForEach(deck => deck.show(fnCheckEnd));
            _game._DeckSwitches.ForEach(deck => deck.show(fnCheckEnd));
            _game._DeckCards.ForEach(deck => deck.show(fnCheckEnd));
        });

        _game._GameBottomMenu.show();
    }

    void onRoundStartAnimationEnd() {
        _game.changeStatus(Game.EStatus.DEAL);
    }

    Game _game;
}
