﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CustomMonoBehavior {
    public void show(System.Action cbEnd = null,
                     float time = 0.5f,
                     iTween.EaseType easeType = iTween.EaseType.easeInCubic) {

        _cbShowEnd = cbEnd;

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", 0,
                                   "to", 1, 
                                   "time", 0.5f,
                                   "easetype", easeType,
                                   "onupdate", "onShowUpdate"));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", 0,
                                   "to", 1, 
                                   "delay", 0.3f,
                                   "time", 0.5f,
                                   "easetype", easeType,
                                   "onupdate", "onShowInfoUpdate",
                                   "oncomplete", "onShowEnd"));
    }

    void onShowUpdate(float alpha) {
        var c = sprite.color;
        c.a = alpha;
        sprite.color = c;
    }

    void onShowInfoUpdate(float alpha) {
        foreach(var sp in GetComponentsInChildren<SpriteRenderer>()) {
            if (sp != sprite) {
                var c = sp.color;
                c.a = alpha;
                sp.color =  c;
            }
        }

        foreach(var m in GetComponentsInChildren<TextMesh>()) {
            var c = m.color;
            c.a = alpha;
            m.color =  c;
        }
    }

    void onShowEnd() {
        if (_cbShowEnd != null) {
            _cbShowEnd();
        }
    }

    protected System.Action _cbShowEnd;

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
    public Card BottomCard { get { return _bottomCard; } }
    public int NumCard { get { return _numCard; } }

    public void putOnCard(Card card) {
        if (TopCard != null) {
            TopCard.putOnCard(card);
        } else {
            _bottomCard = card;
        }

        int num = NumCard;

        changeTopCard(card.TopCard);

        {
            int n = 0;

            card.foreachCardUp(x => {
                x.DeckOn = this;

                x.transform.SetParent(transform, false);
                x.transform.localScale = Vector3.one;
                x.transform.localRotation = Quaternion.identity;
                x.transform.localPosition =
                    Config.Instance.CardStackInitial + CardStackOffset * (num + n++);
            });
        }

        card.foreachCardDown(x => {
            x.transform.localPosition =
                Config.Instance.CardStackInitial + CardStackOffset * --num;
        }, false);
    }

    public virtual Vector3 CardStackOffset { get { return Vector3.zero; } }

    public void getOffCard(Card card) {
        if (isCardExist(card)) {
            changeTopCard(card.DownCard);

            if (card == BottomCard) {
                _bottomCard = null;
            }

            card.getOffCard();
            card.foreachCardUp(x => x.DeckOn = null);

            if (TopCard != null) {
                int num = NumCard;

                TopCard.foreachCardDown(x => {
                    x.transform.localPosition =
                        Config.Instance.CardStackInitial + CardStackOffset * --num;
                });
            }
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
    Card _bottomCard = null;
    int _numCard = 0;
}
