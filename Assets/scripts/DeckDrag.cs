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
            dragUpdate();
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

        _dragOffset = transform.position - MousePosition;

        _draggingCard = card;
        _dragFromDeck = card.DeckOn;

        card.DeckOn.removeCard(card);
		card.transform.SetParent(transform);

        updatePosition();
	}

    void dragEnd() {
        _deckDragOn = null;

        foreach (var deck in Game.Instance._DeckCards) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(deck);
            }
        }

        foreach (var deck in Game.Instance._DeckFinals) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(deck);
            }
        }

        foreach (var deck in Game.Instance._DeckSwitches) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(deck);
            }
        }

        if (_deckDragOn != null && _deckDragOn.canPutOnCard(_draggingCard)) {
            _deckDragOn.addCard(_draggingCard);
        } else if (_dragFromDeck != null) {
            _dragFromDeck.addCard(_draggingCard);
        }

        _deckDragOn = null;
        _draggingCard = null;
        _dragFromDeck = null;

        _isDragging = false;
    }

    void dragOverDeck(Deck deck) {
        float d = Vector3.Distance(MousePosition, deck.transform.position);

        if (_deckDragOn == null || d < _distanceDeckDragOn) {
            _deckDragOn = deck;
            _distanceDeckDragOn = d;
        }
    }

    Deck _deckDragOn = null;
    float _distanceDeckDragOn = 0;

    void dragUpdate() {
        updatePosition();
    }

	void updatePosition() {
		Vector3 pos = MousePosition + _dragOffset;
		pos.z = -1;

		transform.position = pos;
	}

    Vector3 MousePosition {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    Vector3 _dragOffset = Vector3.zero;
    Deck _dragFromDeck = null;
	bool _isDragging = false;
	Card _draggingCard = null;
}
