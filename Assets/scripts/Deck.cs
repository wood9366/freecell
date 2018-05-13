﻿using System.Collections;
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
            return getRect(card.collider2d).Overlaps(getRect(TopCard.collider2d));
        } else {
            return getRect(card.collider2d).Overlaps(getRect(collider2d));
        }
    }

    Rect getRect(Collider2D c) {
        return new Rect(c.bounds.min.x, c.bounds.min.y, c.bounds.size.x, c.bounds.size.y);
    }

    public void empty() {
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

        Card cur = card;

        while (true) {
            cur.DeckOn = this;

            if (cur.UpCard == null) {
                _topCard = cur;
                break;
            }

            cur = cur.UpCard;
        }

        _numCard = calculateNumCard();
    }

    public void removeCard(Card card) {
        if (isCardExist(card)) {
            _topCard = card.DownCard;
            _numCard = calculateNumCard();

            if (_topCard != null) {
                _topCard.removeCard(card);
            }

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