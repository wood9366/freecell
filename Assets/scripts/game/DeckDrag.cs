using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDrag : MonoSingleton<DeckDrag> {

    protected override void init() {
        EventSystem2D.Instance.OnDragStart += onDragStart;
        EventSystem2D.Instance.OnDrag += onDrag;
        EventSystem2D.Instance.OnDragEnd += onDragEnd;
    }

    protected override void release() {
        EventSystem2D.Instance.OnDragStart -= onDragStart;
        EventSystem2D.Instance.OnDrag -= onDrag;
        EventSystem2D.Instance.OnDragEnd -= onDragEnd;
    }

    void onDragStart(Vector3 pos, Collider2D collider) {
        if (IsIgnoreCardDrag) {
            return;
        }

        var card = collider.GetComponent<Card>();

        if (card != null && card.IsDraggable) {
            dragBegin(pos, card);
        }
    }
    
    void onDrag(Vector3 pos, Collider2D collider) {
        if (IsIgnoreCardDrag) {
            return;
        }

        if (_draggingCard != null) {
            dragUpdate(pos);
        }
    }

    void onDragEnd(Vector3 pos, Collider2D collider) {
        if (IsIgnoreCardDrag) {
            return;
        }

        if (_draggingCard != null) {
            dragEnd(pos);
        }
    }

    bool IsIgnoreCardDrag {
        get {
            return Game.Instance.Status != Game.EStatus.GAME
                || IsAutoMoveCardToFinal;
        }
    }

	void dragBegin(Vector3 pos, Card card) {
        _dragOffset = card.transform.position - pos;

        _draggingCard = card;
        _dragFromDeck = card.DeckOn;

        card.DeckOn.getOffCard(card);

        int n = 0;
        
        card.foreachCardUp(x => {
            x.transform.SetParent(transform, false);
            x.transform.localScale = Vector3.one;
            x.transform.localRotation = Quaternion.identity;
            x.transform.localPosition = Config.Instance.CardStackInitial
                + Config.Instance.CardStackOffset * n++;
        });

        updatePosition(pos);
	}

    void dragEnd(Vector3 pos) {
        if (!tryPutCardOnFinalDeck()) {
            if (!tryPutCardOnDragOnDeck(pos)) {
                putCardOnFromDeck();
            }
        }

        autoMoveCardAndSwitchDeckCardToFinalDeck();

        _deckDragOn = null;
        _draggingCard = null;
        _dragFromDeck = null;
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

            card.fly(from, card.transform.position, () => _numAutoMoveFlyCard--,
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

    bool tryPutCardOnDragOnDeck(Vector3 pos) {
        _deckDragOn = null;

        foreach (var deck in Game.Instance._DeckCards) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(pos, deck);
            }
        }

        foreach (var deck in Game.Instance._DeckFinals) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(pos, deck);
            }
        }

        foreach (var deck in Game.Instance._DeckSwitches) {
            if (deck.isOver(_draggingCard)) {
                dragOverDeck(pos, deck);
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

    void dragOverDeck(Vector3 pos, Deck deck) {
        float d = Vector3.Distance(pos, deck.transform.position);

        if (_deckDragOn == null || d < _distanceDeckDragOn) {
            _deckDragOn = deck;
            _distanceDeckDragOn = d;
        }
    }

    Deck _deckDragOn = null;
    float _distanceDeckDragOn = 0;

    void dragUpdate(Vector3 pos) {
        updatePosition(pos);
    }

	void updatePosition(Vector3 pos) {
        float z = transform.position.z;

		pos += _dragOffset;
        pos.z = z;

		transform.position = pos;
	}

    Vector3 _dragOffset = Vector3.zero;
    Deck _dragFromDeck = null;
	Card _draggingCard = null;
}
