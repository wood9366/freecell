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

        return numLinkedCard <= NumGetOffLinkedCard;
    }

    int NumGetOffLinkedCard {
        get {
            return (Game.Instance.NumEmptyCardDeck + 1)
                * (Game.Instance.NumEmptySwitchDeck + 1);
        }
    }

    int NumPutOnLinkedCard {
        get {
            int num = NumGetOffLinkedCard;

            if (TopCard == null) {
                num -= (Game.Instance.NumEmptySwitchDeck + 1);
            }

            return num;
        }
    }

    protected override bool canPutOn(Card card) {
        bool isPutOnRuleOK = TopCard == null || Card.IsLinkedCard(TopCard, card);
        bool isNumPutOnLinkedCardOK = card.NumCardUp <= NumPutOnLinkedCard;

        return isPutOnRuleOK && isNumPutOnLinkedCardOK;
    }

    public override Vector3 CardStackOffset { get { return Config.Instance.CardStackOffset; } }
}
