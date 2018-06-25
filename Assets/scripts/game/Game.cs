using System.Collections.Generic;
using UnityEngine;

public class Game : MonoSingleton<Game> {
    public GameObject _SendDeck = null;
	public List<Deck> _DeckSwitches = new List<Deck>(4);
	public List<DeckFinal> _DeckFinals = new List<DeckFinal>(4);
	public List<DeckCard> _DeckCards = new List<DeckCard>(8);
    public GameMenu _GameMenu;
    public GameTopMenu _GameTopMenu;
    public GameBottomMenu _GameBottomMenu;

    public enum EStatus {
        NONE = 0,
        READY,
        PREPARE,
        DEAL,
        GAME,
        DROP
    }

    public int NumEmptySwitchDeck {
        get {
            int num = 0;
            _DeckSwitches.ForEach(x => { if (x.TopCard == null) num++; });

            return num;
        }
    }

    public int NumEmptyCardDeck {
        get {
            int num = 0;
            _DeckCards.ForEach(x => { if (x.TopCard == null) num++; });

            return num;
        }
    }

    public bool IsAutoPutCardToFinal {
        get { return _isAutoPutCardToFinal; }
        set {
            _isAutoPutCardToFinal = value;

            if (_isAutoPutCardToFinal) {
                _GameTopMenu.SignAuto.show();
                DeckDrag.Instance.autoMoveCardAndSwitchDeckCardToFinalDeck();
            } else {
                _GameTopMenu.SignAuto.hide();
            }
        }
    }

    bool _isAutoPutCardToFinal = false;

    protected override void init() {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        EventSystem2D.Instance.init();

        MoveCardMgr.Instance.OnCommandCursorChange += onCommandCursorChange;

        // init game state machine
        _statuses.addState((int)EStatus.READY, new GameStateReady(this));
        _statuses.addState((int)EStatus.PREPARE, new GameStatePrepare(this));
        _statuses.addState((int)EStatus.DEAL, new GameStateDeal(this));
        _statuses.addState((int)EStatus.GAME, new GameStatePlay(this));
        _statuses.addState((int)EStatus.DROP, new GameStateDrop(this));

        shuffleRoundCards();
        changeStatus(EStatus.READY);
    }

    void onCommandCursorChange() {
        _GameBottomMenu._ButtonUndo.Enabled = MoveCardMgr.Instance.CanUndo;
    }

    protected override void release() {
        EventSystem2D.Instance.release();
        MoveCardMgr.Instance.OnCommandCursorChange -= onCommandCursorChange;
    }

    void Update() {
        InputMgr.Instance.update();
    }

    public void restart() {
        if (Status == EStatus.GAME) {
            changeStatus(EStatus.DROP);
        }
    }

    public void newRound() {
        shuffleRoundCards();

        if (Status == EStatus.GAME) {
            changeStatus(EStatus.DROP);
        } else {
            changeStatus(EStatus.READY);
        }
    }

    public void changeStatus(EStatus status) {
        _statuses.setState((int)status);
    }

    public void resetRound() {
        foreach (var card in _cards) {
            GameObject.DestroyImmediate(card.gameObject);
        }
        _cards.Clear();

		foreach (var deck in _DeckSwitches) {
			deck.empty();
		}

		foreach (var deck in _DeckFinals) {
			deck.empty();
		}

		foreach (var deck in _DeckCards) {
			deck.empty();
		}

        MoveCardMgr.Instance.reset();
    }

    // create cards game object and put on round deckes
    public void prepareRoundCards() {
        _roundCards.forEach((deckIdx, deckCardIdx, cardIdx, cardId) => {
            _DeckCards[deckIdx].putOnCard(createCard(cardId));
        });
    }

    void shuffleRoundCards() {
        _roundCards.shuffle();
    }

    RoundCards _roundCards = new RoundCards();

    Card createCard(int id) {
        var card = GameObject.Instantiate(ResourceMgr.Instance.getCardPrefab(),
                                          _SendDeck.transform, false);

        card.init(id);

        _cards.Add(card);

		return card;
	}

    public List<Card> Cards { get { return _cards; } }

    List<Card> _cards = new List<Card>();

    public EStatus Status { get { return (EStatus)_statuses.CurrentStateId; } }

    StateMachine _statuses = new StateMachine();

#if UNITY_EDITOR
	[ContextMenu("Auto Set Deck")]
	void autoSetDeck() {
		setDeck<Deck>("2d/Desk/SwitchDecks", _DeckSwitches);
		setDeck<DeckFinal>("2d/Desk/FinalDecks", _DeckFinals);
		setDeck<DeckCard>("2d/Desk/CardDecks", _DeckCards);

        var types = new CardData.ECardType[] {
            CardData.ECardType.SPADE,
            CardData.ECardType.HEART,
            CardData.ECardType.CLUB,
            CardData.ECardType.DIAMOND
        };

        for (int i = 0; i < _DeckFinals.Count; i++) {
            if (i < types.Length) {
                _DeckFinals[i].CardType = types[i];
            }
        }
	}

	void setDeck<T>(string path, List<T> _deckes) {
		var obj = GameObject.Find(path);

		if (obj != null) {
            var grid = obj.GetComponent<LayoutGrid>();

            if (grid != null) {
                grid.delete();
                grid.create();
            }

			var deckes = obj.GetComponentsInChildren<T>();

			_deckes.Clear();

			foreach (var deck in deckes) {
				_deckes.Add(deck);
			}
		}
	}
#endif
}
