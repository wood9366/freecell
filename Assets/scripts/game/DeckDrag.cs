using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrag : MonoSingleton<DeckDrag> {

    void onMouseUpCard(Card card) {
        if (IsIgnoreCardDrag) {
            return;
        }

		if (_isDragging) {
            dragEnd();
		}
    }

    void onMouseDragCard(Card card) {
        if (IsIgnoreCardDrag) {
            return;
        }

		if (_isDragging) {
            dragUpdate();
		} else if (card.IsDraggable) {
            dragBegin(card);
        }
    }

    bool IsIgnoreCardDrag {
        get { return Game.Instance.Status != Game.EStatus.GAME || IsAutoMoveCardToFinal; }
    }

	void dragBegin(Card card) {
		_isDragging = true;

        _dragOffset = card.transform.position - MousePosition;

        _draggingCard = card;
        _dragFromDeck = card.DeckOn;

        card.DeckOn.getOffCard(card);

        int n = 0;
        
        card.foreachCardUp(x => {
            x.transform.SetParent(transform, false);
            x.transform.localScale = Vector3.one;
            x.transform.localRotation = Quaternion.identity;
            x.trans2d.anchoredPosition3D =
                Config.Instance.CardStackInitial + Config.Instance.CardStackOffset * n++;
        });

        updatePosition();
	}

    void dragEnd() {
        if (!tryPutCardOnFinalDeck()) {
            if (!tryPutCardOnDragOnDeck()) {
                putCardOnFromDeck();
            }
        }

        autoMoveCardAndSwitchDeckCardToFinalDeck();

        _deckDragOn = null;
        _draggingCard = null;
        _dragFromDeck = null;

        _isDragging = false;
    }

    public void autoMoveCardAndSwitchDeckCardToFinalDeck() {
        prepareForAutoMoveCardToFinalDeck();

        bool isDeckChange = true;

        while (isDeckChange) {
            isDeckChange = false;

            foreach (var deck in Game.Instance._DeckCards) {
                isDeckChange = tryMoveDeckCardToFinalDeck(deck) || isDeckChange;
            }

            foreach (var deck in Game.Instance._DeckSwitches) {
                isDeckChange = tryMoveDeckCardToFinalDeck(deck) || isDeckChange;
            }
        }
    }

    bool tryMoveDeckCardToFinalDeck(Deck deck) {
        if (deck.TopCard == null) {
            return false;
        }

        var finalDeck = findFinalDeckCardCanPutOn(deck.TopCard);

        if (finalDeck != null) {
            var card = deck.TopCard;

            Vector3 from = card.transform.position;

            deck.getOffCard(card);
            finalDeck.putOnCard(card);

            Vector3 pos = card.transform.position;

            card.fly(from, card.transform.position,
                     () => { _numAutoMoveFlyCard--; card.transform.position = pos; },
                     _autoMoveCardFlyDelay, _autoMoveCardFlyZ);

            _numAutoMoveFlyCard++;
            _autoMoveCardFlyDelay += 0.1f;
            _autoMoveCardFlyZ += -0.1f;

            return true;
        }

        return false;
    }

    void prepareForAutoMoveCardToFinalDeck() {
        _autoMoveCardFlyDelay = 0;
        _autoMoveCardFlyZ = 0;
        _numAutoMoveFlyCard = 0;
    }

    bool IsAutoMoveCardToFinal { get { return _numAutoMoveFlyCard > 0; } }

    float _autoMoveCardFlyDelay = 0;
    float _autoMoveCardFlyZ = 0;
    int _numAutoMoveFlyCard = 0;

    void putCardOnFromDeck() {
        if (_dragFromDeck != null) {
            _dragFromDeck.putOnCard(_draggingCard);
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
            _deckDragOn.putOnCard(_draggingCard);

            return true;
        } 

        return false;
    }

    bool tryPutCardOnFinalDeck() {
        var deck = findFinalDeckCardCanPutOn(_draggingCard);

        if (deck != null) {
            deck.putOnCard(_draggingCard);

            return true;
        }

        return false;
    }

    Deck findFinalDeckCardCanPutOn(Card card) {
        foreach (Deck deck in Game.Instance._DeckFinals) {
            if (deck.canPutOnCard(card)) {
                return deck;
            }
        }

        return null;
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
