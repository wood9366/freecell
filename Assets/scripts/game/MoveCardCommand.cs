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
        Vector3 from = _moveCard.transform.position;

        _deckTo.getOffCard(_moveCard);
        _deckFrom.putOnCard(_moveCard);

        _moveCard.fly(from, _moveCard.transform.position, null, 0, 0, false, 10,
                      iTween.EaseType.easeOutExpo);
    }

    bool _isRedo = false;
    Deck _deckFrom;
    Card _moveCard;
    Deck _deckTo;
}
