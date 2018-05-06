using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard : MonoBehaviour {
	public CardObject _CardObjectTemplate;
	public Vector2 _CardObjectInitialPos = new Vector2(0, -60);
	public float _CardObjectOffsetY = -50;
	public int _NumMaxCard = 16;

	public void empty() {
		foreach (var card in _Cards) {
			GameObject.DestroyImmediate(card.gameObject);
		}

		_Cards.Clear();
	}

	public void moveCardIn(Card card) {
		if (_Cards.Count < _NumMaxCard) {
			createCardObject(card);
		} else {
			Debug.LogFormat("can't move card {0} in {1}, out of max nubmer {2}",
				card.ToString(), name, _NumMaxCard);
		}
	}

	CardObject createCardObject(Card card) {
		Vector3 pos = Vector3.zero;

		if (_Cards.Count > 0) {
			pos = _Cards[_Cards.Count - 1].transform.localPosition;
			pos.y += _CardObjectOffsetY;
		} else {
			pos = _CardObjectInitialPos;
		}

		var obj = GameObject.Instantiate(_CardObjectTemplate,
			Vector3.zero, Quaternion.identity, transform);

		obj.transform.localPosition = pos;

		CardObject cardObj = obj.GetComponent<CardObject>();
		cardObj.set(card);

		_Cards.Add(cardObj);

		return cardObj;
	}

	List<CardObject> _Cards = new List<CardObject>();
}
