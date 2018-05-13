using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrag : MonoBehaviour {

	static public DeckDrag Instance { get { return sInstance; } }
	static DeckDrag sInstance = null;

	void Awake() {
		sInstance = this;
	}

    void onMouseUpCard(Card card) {
		// Debug.Log("end drag " + cardObj.Data.ToString());
		if (_isDragging && card == _draggingCard) {
            dragEnd();
		}
    }

    void onMouseDragCard(Card card) {
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

    void click(Card card) {
        foreach (Deck deck in Game.Instance._DeckFinals) {
            if (deck.canPutOnCard(card)) {
                deck.addCard(card);
                break;
            }
        }
    }

	void dragBegin(Card card) {
		_isDragging = true;

        _dragOffset = transform.position - MousePosition;

        _draggingCard = card;
        _dragFromDeck = card.DeckOn;

        card.DeckOn.removeCard(card);
		card.transform.SetParent(transform);

        Vector3 pos = card.transform.localPosition;
        pos.z = -0.1f;

        card.transform.localPosition = pos;

        updatePosition();
	}

    void dragEnd() {
        if (!tryPutCardOnFinalDeck()) {
            if (!tryPutCardOnDragOnDeck()) {
                putCardOnFromDeck();
            }
        }

        _deckDragOn = null;
        _draggingCard = null;
        _dragFromDeck = null;

        _isDragging = false;
    }

    void putCardOnFromDeck() {
        if (_dragFromDeck != null) {
            _dragFromDeck.addCard(_draggingCard);
        }
    }

    bool tryPutCardOnDragOnDeck() {
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

            return true;
        } 

        return false;
    }

    bool tryPutCardOnFinalDeck() {
        foreach (Deck deck in Game.Instance._DeckFinals) {
            if (deck.canPutOnCard(_draggingCard)) {
                deck.addCard(_draggingCard);

                return true;
            }
        }

        return false;
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
        float z = transform.position.z;

		Vector3 pos = MousePosition + _dragOffset;
        pos.z = z;

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
