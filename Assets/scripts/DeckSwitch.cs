using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSwitch : MonoBehaviour {
	public CardObject _Card;

	public Card card {
		get { return _Card.Data; }
	}

	public bool IsEmpty {
		get {
			return _Card.Data.IsValid;
		}
	}

	public void empty() {
		_Card.IsEnabled = false;
	}

	public void moveCardIn(Card card) {
		if (!IsEmpty) {
			Debug.LogWarning("can't move card in to switch deck has card in");
			return;
		}

		if (card.IsValid) {
			_Card.set(card);
		} else {
			Debug.LogWarningFormat("try to move invalid card in desk ({0})", name);
		}
	}

	public Card moveCardOut() {
		_Card.IsEnabled = false;

		return _Card.Data;
	}
}
