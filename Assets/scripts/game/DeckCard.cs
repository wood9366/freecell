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

        while (up != null) {
            if (Card.IsLinkedCard(down, up)) {
                down = up;
                up = up.UpCard;
            } else {
                return false;
            }
        }

        return true;
    }

    protected override bool canPutOn(Card card) {
        return TopCard == null || Card.IsLinkedCard(TopCard, card);
    }

    protected override Vector3 CardStackOffset { get { return Config.Instance.CardStackOffset; } }
}
