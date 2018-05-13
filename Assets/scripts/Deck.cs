using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CustomMonoBehavior {
    public virtual int MaxNumCard { get { return 1; } }
    public virtual bool isDraggable(Card card) { return isCardExist(card); }

    public bool canPutOnCard(Card card) {
        return NumCard + card.NumCardOn <= MaxNumCard && _canPutOnCard(card);
    }

    protected virtual bool _canPutOnCard(Card card) { return true; }

    public bool isOver(Card card) {
        if (TopCard != null) {
            return ColliderUtility.IsColliderOverlap(card.collider2d, TopCard.collider2d);
        } else {
            return ColliderUtility.IsColliderOverlap(card.collider2d, collider2d);
        }
    }

    public void empty() {
        if (_topCard != null) {
            _topCard.foreachCardDown(x => GameObject.DestroyImmediate(x.gameObject));
        }

        _topCard = null;
        _numCard = 0;
    }

    protected virtual Vector3 CardStackOffset { get { return Vector3.zero; } }

    public void addCard(Card card) {
        if (_topCard != null) {
            _topCard.putOnCard(card, CardStackOffset);
        } else {
            // first card set position on deck
            card.transform.SetParent(transform, false);
            card.transform.localScale = Vector3.one;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localPosition = new Vector3(0, 0, -0.1f);
        }

        card.foreachCardUp(x => {
            x.DeckOn = this;
            if (x.UpCard == null) _topCard = x;
        });

        _numCard = calculateNumCard();
    }

    public void removeCard(Card card) {
        if (isCardExist(card)) {
            _topCard = card.DownCard;
            _numCard = calculateNumCard();

            card.getOffCard();
            card.foreachCardUp(x => x.DeckOn = null);
        }
    }

    public Card TopCard { get { return _topCard; } }

    int NumCard { get { return _numCard; } }

    int calculateNumCard() {
        int num = 0;

        if (TopCard != null) {
            TopCard.foreachCardDown(x => num++);
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
