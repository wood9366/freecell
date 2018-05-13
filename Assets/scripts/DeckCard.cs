using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard : Deck {
    public override int MaxNumCard { get { return 13; } }

    public override bool canGetOffCard(Card card) {
        if (isCardExist(card)) {
            if (card == TopCard) {
                return true;
            } else {
                Card prev = card;
                Card cur = card.UpCard;

                while (cur != null) {
                    bool isDifferentTypeColor = cur.CardTypeColor != prev.CardTypeColor;
                    bool isStepIncreasedVal = cur.CardVal + 1 == prev.CardVal;

                    if (isDifferentTypeColor && isStepIncreasedVal) {
                        prev = cur;
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

    protected override bool canPutOn(Card card) {
        if (TopCard == null) {
            return true;
        }

        bool isDifferentTypeColor = card.CardTypeColor != TopCard.CardTypeColor;
        bool isCorrectValue = card.CardVal + 1 == TopCard.CardVal;

        return isDifferentTypeColor && isCorrectValue;
    }

    protected override Vector3 CardStackOffset { get { return Config.Instance.CardStackOffset; } }
}
