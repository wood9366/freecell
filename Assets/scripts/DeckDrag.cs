using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrag : MonoBehaviour {

	static public DeckDrag Instance { get { return sInstance; } }
	static DeckDrag sInstance = null;

	void Awake() {
		sInstance = this;
	}

	void onDragCard(Card card) {
		if (_isDragging) {
            // Debug.Log("dargging " + card.CardInfo());
            updatePosition();
		} else {
            // Debug.Log("start drag " + card.CardInfo());

			if (card.IsDraggable) {
                dragBegin(card);
			}
        }
	}

	void onDragCardEnd(Card cardObj) {
		// Debug.Log("end drag " + cardObj.Data.ToString());

		if (_isDragging && cardObj == _draggingCard) {
            dragEnd();
		}
	}

	void dragBegin(Card card) {
		_isDragging = true;

        _draggingCard = card;
        _dragFromDeck = card.DeckOn;

        card.DeckOn.putOffCard(card);

		card.transform.SetParent(transform);
		card.transform.localPosition = new Vector3(0, 0, -0.1f);
		card.transform.localRotation = Quaternion.identity;
		card.transform.localScale = Vector3.one;

        updatePosition();
	}

    void dragEnd() {
        if (_dragFromDeck != null) {
            _dragFromDeck.addCard(_draggingCard);
        }

        _draggingCard = null;
        _dragFromDeck = null;

        _isDragging = false;
    }

	void updatePosition() {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = -1;

		transform.position = pos;
	}

    Deck _dragFromDeck = null;
	bool _isDragging = false;
	Card _draggingCard = null;
}
