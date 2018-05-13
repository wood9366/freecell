using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckFinal : Deck {
    public CardData.ECardType _CardType;

    public override int MaxNumCard { get { return 13; } }

    public override bool canGetOffCard(Card card) { return false; }

    protected override bool canPutOn(Card card) {
        if (card.CardType == _CardType) {
            if (TopCard == null) {
                return card.CardVal == 0;
            } else {
                return card.CardVal == TopCard.CardVal + 1;
            }
        }

        return false;
    }
}
