using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardCommand : ICommand {
    public MoveCardCommand(Card moveCard, Deck deckTo) {
        _deckFrom = moveCard.DeckOn;
        _moveCard = moveCard;
        _deckTo = deckTo;
    }

    public void run() {
        Vector3 from = _moveCard.transform.position;

        _deckFrom.getOffCard(_moveCard);
        _deckTo.putOnCard(_moveCard);

        if (_isRedo) {
            _moveCard.fly(from, _moveCard.transform.position, null, 0, 0, false, 10,
                        iTween.EaseType.easeOutExpo);
        } else {
            _isRedo = true;
        }
    }

    public void undo() {
        List<FlyCard> flyCards = new List<FlyCard>();

        _moveCard.foreachCardUp(card => {
            FlyCard flyCard;

            flyCard.card = card;
            flyCard.from = card.transform.position;

            flyCards.Add(flyCard);
        });

        _deckTo.getOffCard(_moveCard);
        _deckFrom.putOnCard(_moveCard);

        foreach (var flyCard in flyCards) {
            flyCard.card.fly(flyCard.from, flyCard.card.transform.position, null, 0, 0, false, 10,
                             iTween.EaseType.easeOutExpo);
        }
    }

    struct FlyCard {
        public Card card;
        public Vector3 from;
    }

    bool _isRedo = false;
    Deck _deckFrom;
    Card _moveCard;
    Deck _deckTo;
}
