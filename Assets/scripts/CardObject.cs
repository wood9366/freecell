using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : CustomMonoBehavior {
	public SpriteRenderer _SpriteType;
	public SpriteRenderer _SpriteVal;

	public Card Data { get { return _card; } }

	public DeckSwitch deckSwitch {
		get { return _deckSwitch; }
		set {
			_deckSwitch = value;
			_deckCard = null;
		}
	}

	public DeckCard deckCard {
		get { return _deckCard; }
		set {
			_deckCard = value;
			_deckSwitch = null;
		}
	}

	DeckSwitch _deckSwitch = null;
	DeckCard _deckCard = null;

	public void set(Card card) {
		var isValid = card.IsValid;

		IsEnabled = isValid;

		if (isValid) {
			_card.set(card);

			_SpriteType.sprite = ResourceMgr.Instance.getSpriteCardType(_card.Type);
			_SpriteVal.color = ResourceMgr.Instance.getColorCardType(_card.Type);
			_SpriteVal.sprite = ResourceMgr.Instance.getSpriteCardVal(_card.Val);
		}

		setName();
	}

	public bool IsEnabled {
		get { return gameObject.activeInHierarchy; }
		set { gameObject.SetActive(value); }
	}

	void setName() {
        name = Data.Type.ToString() + "_" + Data.Val.ToString();
	}

	void Awake() {
		setName();
		IsEnabled = false;
	}

	void OnMouseDrag() {
		DragDeckCard.Instance.SendMessage("onDragCard", this);
	}

	void OnMouseUp() {
		DragDeckCard.Instance.SendMessage("onDragCardEnd", this);
	}

	Card _card = new Card();
}
