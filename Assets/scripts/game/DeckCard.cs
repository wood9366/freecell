using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard : Deck {
    public override int MaxNumCard { get { return 13; } }

    public override bool canGetOffCard(Card card) {
        if (!isCardExist(card)) {
            return false;
        }

        Card down = card;
        Card up = card.UpCard;
        int numLinkedCard = 1;

        while (up != null) {
            if (Card.IsLinkedCard(down, up)) {
                numLinkedCard++;
                down = up;
                up = up.UpCard;
            } else {
                return false;
            }
        }

        return numLinkedCard <= NumMoveLinkedCard;
    }

    int NumMoveLinkedCard {
        get { return Game.Instance.NumEmptyCardDeck + Game.Instance.NumEmptySwitchDeck + 1; }
    }

    protected override bool canPutOn(Card card) {
        return TopCard == null || Card.IsLinkedCard(TopCard, card);
    }

    protected override Vector3 CardStackOffset { get { return Config.Instance.CardStackOffset; } }
}
