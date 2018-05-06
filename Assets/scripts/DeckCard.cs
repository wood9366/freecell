using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard : MonoBehaviour {
	public CardObject _CardObjectTemplate;
	public Vector3 _CardObjectInitialPos = new Vector3(0, -60, -0.1f);
	public Vector3 _CardObjectOffset = new Vector3(0, -50, -0.1f);
	public int _NumMaxCard = 16;

	public void empty() {
		foreach (var card in _Cards) {
			GameObject.DestroyImmediate(card.gameObject);
		}

		_Cards.Clear();
	}

	public void addCard(Card card) {
		if (_Cards.Count < _NumMaxCard) {
			createCardObject(card);
		} else {
			Debug.LogFormat("can't move card {0} in {1}, out of max nubmer {2}",
				card.ToString(), name, _NumMaxCard);
		}
	}

	public bool isDragable(CardObject card) {
		return TopCardObject == card;
	}

	public void dragCardIn(CardObject cardObj) {
		addCardObject(cardObj);
	}

	public void dragCardOut(CardObject cardObj) {
        removeCardObject(cardObj);
	}

	void addCardObject(CardObject card) {
		if (!_Cards.Contains(card)) {
			card.deckCard = this;

			card.transform.SetParent(transform);
			card.transform.localScale = Vector3.one;
			card.transform.localRotation = Quaternion.identity;
			card.transform.localPosition = NextCardLocalPosition;

			_Cards.Add(card);
		}
	}

	Vector3 NextCardLocalPosition {
		get {
            if (_Cards.Count > 0) {
                return _Cards[_Cards.Count - 1].transform.localPosition + _CardObjectOffset;
            } else {
                return _CardObjectInitialPos;
            }
		}
	}

	void removeCardObject(CardObject card) {
		_Cards.Remove(card);
	}

	CardObject createCardObject(Card card) {
		var cardObj = GameObject.Instantiate(_CardObjectTemplate,
			Vector3.zero, Quaternion.identity);

		addCardObject(cardObj);

		cardObj.set(card);

		return cardObj;
	}

	CardObject TopCardObject {
		get {
			return _Cards.Count > 0 ? _Cards[_Cards.Count - 1] : null;
		}
	}

	List<CardObject> _Cards = new List<CardObject>();
}
