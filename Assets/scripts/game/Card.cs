using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : CustomMonoBehavior {
	public SpriteRenderer _SpriteType;
	public SpriteRenderer _SpriteVal;

    static public bool IsLinkedCard(Card down, Card up) {
        bool isDifferentTypeColor = up.CardTypeColor != down.CardTypeColor;
        bool isStepIncreasedVal = up.CardVal + 1 == down.CardVal;

        return isDifferentTypeColor && isStepIncreasedVal;
    }

	public void init(int id) {
        _card = CardData.Create(id);

		if (_card != null) {
			_SpriteType.sprite = ResourceMgr.Instance.getSpriteCardType(_card.Type);
			_SpriteVal.color = ResourceMgr.Instance.getColorCardType(_card.Type);
			_SpriteVal.sprite = ResourceMgr.Instance.getSpriteCardVal(_card.Val);

            name = CardType.ToString() + "_" + (CardVal + 1);
		}
	}

    public void fly(Vector3 from, Vector3 to, System.Action listener,
                    float delay = 0,
                    float zoffset = 0)
    {
        if (_isFlying) {
            return;
        }

        to.z = from.z = Game.Instance._SendDeck.transform.position.z
            + zoffset;

        _flyCompleteListeners = listener;

        transform.position = to;

        iTween.MoveFrom(gameObject,
                        iTween.Hash("delay", delay,
                                    "position", from,
                                    "speed", Config.Instance.CardFlySpeed,
                                    "oncomplete", "onFlyComplete"));

        _isFlying = true;
    }

    void onFlyComplete() {
        if (_flyCompleteListeners != null) {
            _flyCompleteListeners();
            _flyCompleteListeners = null;
        }

        _isFlying = false;
    }

    System.Action _flyCompleteListeners = null;
    bool _isFlying = false;

    public Deck DeckOn {
        get { return _deck; }
        set { _deck = value; }
    }

    public bool IsDraggable {
        get {
            return DeckOn != null && DeckOn.canGetOffCard(this);
        }
    }

    public string CardInfo { get { return _card.ToString(); } }

    public CardData.ECardTypeColor CardTypeColor { get { return _card.TypeColor; } }
    public CardData.ECardType CardType { get { return _card.Type; } }
    public int CardVal { get { return _card.Val; } }

    Deck _deck = null;
	CardData _card = null;

    #region Card Pipe
    public bool IsTopCard { get { return _upCard == null; } }

    public int NumCardUp {
        get {
            int num = 0;
            foreachCardUp(x => num++);

            return num;
        }
    }

    public Card UpCard { get { return _upCard; } }
    public Card DownCard { get { return _downCard; } }

    public void putOnCard(Card card) {
        _upCard = card;
        card._downCard = this;
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

    Card _upCard = null;
    Card _downCard = null;
    #endregion

    #region Mouse Event
	void OnMouseDrag() {
		DeckDrag.Instance.SendMessage("onMouseDragCard", this);
	}

	void OnMouseUp() {
		DeckDrag.Instance.SendMessage("onMouseUpCard", this);
	}
    #endregion
}
