using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard : Deck {
    public override int MaxNumCard { get { return 13; } }

    public override bool canPutOnCard(Card card) {
        if (TopCard == null) {
            return true;
        }

        bool isDifferentTypeColor = card.CardTypeColor != TopCard.CardTypeColor;
        bool isCorrectValue = card.CardVal == TopCard.CardVal + 1;

        return isDifferentTypeColor && isCorrectValue;
    }

    public override bool isDraggable(Card card) {
        if (isCardExist(card)) {
            if (card == TopCard) {
                return true;
            } else {
                Card prev = card;
                Card cur = card.UpCard;

                while (cur != null) {
                    bool isDifferentTypeColor = cur.CardTypeColor != prev.CardTypeColor;
                    bool isStepIncreasedVal = cur.CardVal == prev.CardVal + 1;

                    if (isDifferentTypeColor && isStepIncreasedVal) {
                        cur = cur.UpCard;
                    } else {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }
}
