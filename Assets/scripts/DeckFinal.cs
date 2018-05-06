using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckFinal : MonoBehaviour {
	public CardObject _Card;
	public Card.ECardType _CardType;

	public Card TopCard {
		get {
			return _Card.Data;
		}
	}
	
	public bool IsEmpty {
		get {
			return !_Card.Data.IsValid;
		}
	}

	public void empty() {
		_Card.IsEnabled = false;
	}

	public void moveCardIn(Card card) {
		if (isNextCard(card)) {
			if (IsEmpty) {
				_Card.IsEnabled = true;
			}

			_Card.set(card);
		} else {
			Debug.LogWarningFormat("can't move card {0} in deck final {1}",
				card.ToString(), ToString());
		}
	}

	override public string ToString() {
		return string.Format("Desk Final {0} top {1}", CardType, TopCard.Val.ToString());
	}

	public bool isNextCard(Card card) {
		return card.Type == CardType
			&& card.Val == (TopCard.IsValid ? TopCard.Val + 1 : 1);
	}

	Card.ECardType CardType { get { return _CardType; } }
}
