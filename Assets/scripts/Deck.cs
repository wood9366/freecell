using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    public Vector3 _InitialPosition = new Vector3(0, -120, 0);

    public virtual int MaxNumCard { get { return 1; } }
    public virtual bool canPutOnCard(Card card) { return true; }
    public virtual bool isDraggable(Card card) { return isCardExist(card); }

    public void empty() {
        _topCard = null;
        _numCard = 0;
    }

    public void putOnCard(Card card) {
        if (NumCard + card.NumCardOn <= MaxNumCard && canPutOnCard(card)) {
            addCard(card);
        }
    }

    public void addCard(Card card) {
        if (_topCard != null) {
            _topCard.putOnCard(card);
        } else {
            // first card set position on deck
            card.transform.SetParent(transform, false);
            card.transform.localScale = Vector3.one;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localPosition = _InitialPosition;
        }

        _topCard = card;
        _numCard = calculateNumCard();

        Card cur = card;

        while (cur != null) {
            cur.DeckOn = this;
            cur = cur.UpCard;
        }
    }

    public void putOffCard(Card card) {
        if (isCardExist(card)) {
            _topCard = card.DownCard;
            _numCard = calculateNumCard();

            Card cur = card;

            while (cur != null) {
                cur.DeckOn = null;
                cur = cur.UpCard;
            }
        }
    }

    public Card TopCard { get { return _topCard; } }

    int NumCard { get { return _numCard; } }

    int calculateNumCard() {
        int num = 0;

        var cur = TopCard;

        while (cur != null) {
            num++;
            cur = cur.DownCard;
        }

        return num;
    }

    protected bool isCardExist(Card card) {
        var cur = TopCard;

        while (cur != null) {
            if (card == cur) {
                return true;
            }

            cur = cur.DownCard;
        }

        return false;
    }

    Card _topCard = null;
    int _numCard = 0;
}
