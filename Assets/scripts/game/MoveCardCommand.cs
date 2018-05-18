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
        _deckFrom.getOffCard(_moveCard);
        _deckTo.putOnCard(_moveCard);
    }

    public void undo() {
        _deckTo.getOffCard(_moveCard);
        _deckFrom.putOnCard(_moveCard);
    }

    Deck _deckFrom;
    Card _moveCard;
    Deck _deckTo;
}
