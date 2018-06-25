using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDeal : State {
    public GameStateDeal(Game game) {
        _game = game;
    }

    public override void enter() {
        _game.resetRound();
        _game.prepareRoundCards();

        _game._GameTopMenu.setRoundTime(0);
        _game._GameBottomMenu._ButtonUndo.Enabled = MoveCardMgr.Instance.CanUndo;
        _game._GameMenu._ButtonNewRound.Enabled = false;
        _game._GameMenu._ButtonRestart.Enabled = false;

        dealCards();
    }

    void dealCards() {
        DebugUtility.Assert(_game.Cards.Count > 0);

        float delayFlyCard = 0.5f;
        float zOffsetFlyCard = 0;

        _numFlyCard = _game.Cards.Count;

        _game._DeckCards.ForEach(deck => {
            if (deck.BottomCard != null) {
                deck.BottomCard.foreachCardUp(card => {
                    card.fly(_game._SendDeck.transform.position,
                             card.transform.position,
                             onCardFlyEnd,
                             delayFlyCard, zOffsetFlyCard,
                             true, 0,
                             iTween.EaseType.easeOutExpo);

                    zOffsetFlyCard += -0.1f;
                    delayFlyCard += Config.Instance.DealCardInterval;
                });
            }
        });
    }

    void onCardFlyEnd() {
        if (--_numFlyCard == 0) {
            dealEnd();
        }
    }

    void dealEnd() {
        _game.changeStatus(Game.EStatus.GAME);
    }

    int _numFlyCard = 0;
    Game _game;
}
