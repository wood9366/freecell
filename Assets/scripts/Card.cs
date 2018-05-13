using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : CustomMonoBehavior {
	public SpriteRenderer _SpriteType;
	public SpriteRenderer _SpriteVal;

	public void init(int id) {
        _card = CardData.Create(id);

		if (_card != null) {
			_SpriteType.sprite = ResourceMgr.Instance.getSpriteCardType(_card.Type);
			_SpriteVal.color = ResourceMgr.Instance.getColorCardType(_card.Type);
			_SpriteVal.sprite = ResourceMgr.Instance.getSpriteCardVal(_card.Val);

            name = CardType.ToString() + "_" + CardVal.ToString();
		}
	}

    public Deck DeckOn {
        get { return _deck; }
        set { _deck = value; }
    }

    public bool IsDraggable {
        get {
            return DeckOn != null && DeckOn.isDraggable(this);
        }
    }

    public string CardInfo { get { return _card.ToString(); } }

    public CardData.ECardTypeColor CardTypeColor { get { return _card.TypeColor; } }
    public CardData.ECardType CardType { get { return _card.Type; } }
    public int CardVal { get { return _card.Val; } }

    public bool IsTopCard { get { return _upCard == null; } }

    public int NumCardOn {
        get {
            int num = 0;

            Card cur = this;

            while (cur != null) {
                num++;
                cur = cur.UpCard;
            }

            return num;
        }
    }

    public Card UpCard { get { return _upCard; } }
    public Card DownCard { get { return _downCard; } }

    public void putOnCard(Card card, Vector3 offset) {
        _upCard = card;
        card._downCard = this;

        card.transform.SetParent(transform);
        card.transform.localScale = Vector3.one;
        card.transform.localRotation = Quaternion.identity;
        card.transform.localPosition = offset;
    }

    public void putOnCard(Card card) {
        putOnCard(card, Vector3.zero);
    }

    public void getOffCard() {
        if (DownCard != null) {
            DownCard._upCard = null;
        }

        _downCard = null;
    }

    public void foreachCardUp(System.Action<Card> fn, bool includeSelf = true) {
        var cur = includeSelf ? this : UpCard;

        while (cur != null) {
            fn(cur);
            cur = cur.UpCard;
        }
    }

    public void foreachCardDown(System.Action<Card> fn, bool includeSelf = true) {
        var cur = includeSelf ? this : DownCard;

        while (cur != null) {
            fn(cur);
            cur = cur.DownCard;
        }
    }

	void OnMouseDrag() {
		DeckDrag.Instance.SendMessage("onMouseDragCard", this);
	}

	void OnMouseUp() {
		DeckDrag.Instance.SendMessage("onMouseUpCard", this);
	}

    Deck _deck = null;
	CardData _card = null;
    Card _upCard = null;
    Card _downCard = null;
}
