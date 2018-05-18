using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CustomMonoBehavior {
    public virtual int MaxNumCard { get { return 1; } }

    public virtual bool canGetOffCard(Card card) { return isCardExist(card); }

    public bool canPutOnCard(Card card) {
        return hasEnoughSpacePutOn(card) && canPutOn(card);
    }

    bool hasEnoughSpacePutOn(Card card) {
        return NumCard + card.NumCardUp <= MaxNumCard;
    }

    protected virtual bool canPutOn(Card card) { return true; }

    public bool isOver(Card card) {
        if (TopCard != null) {
            return ColliderUtility.IsColliderOverlap(card.collider2d, TopCard.collider2d);
        } else {
            return ColliderUtility.IsColliderOverlap(card.collider2d, collider2d);
        }
    }

    public void empty() {
        changeTopCard(null, true);
    }

    public Card TopCard { get { return _topCard; } }
    public int NumCard { get { return _numCard; } }

    public void putOnCard(Card card) {
        if (TopCard != null) {
            TopCard.putOnCard(card);
        }

        int n = 0;

        card.foreachCardUp(x => {
            x.DeckOn = this;

            x.transform.SetParent(transform, false);
            x.transform.localScale = Vector3.one;
            x.transform.localRotation = Quaternion.identity;
            x.transform.localPosition =
                Config.Instance.CardStackInitial + CardStackOffset * (NumCard + n++);

            if (x.UpCard == null) changeTopCard(x);
        });
    }

    public virtual Vector3 CardStackOffset { get { return Vector3.zero; } }

    public void getOffCard(Card card) {
        if (isCardExist(card)) {
            changeTopCard(card.DownCard);

            card.getOffCard();
            card.foreachCardUp(x => x.DeckOn = null);
        }
    }

    void changeTopCard(Card top, bool force = false) {
        if (force || top != _topCard) {
            _topCard = top;

            calculateNumCard();
        }
    }

    void calculateNumCard() {
        _numCard = 0;

        if (TopCard != null) {
            TopCard.foreachCardDown(x => _numCard++);
        }
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
