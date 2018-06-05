using System.Collections;
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

        changeStatus(EStatus.READY);
    }

    void onCommandCursorChange() {
        _GameBottomMenu._ButtonUndo.Enabled = MoveCardMgr.Instance.CanUndo;
    }

    public void restart() {
        if (Status == EStatus.GAME) {
            _isRestart = true;
            changeStatus(EStatus.DROP);
        }
    }

    bool _isRestart = false;

    public void newRound() {
        if (Status == EStatus.GAME) {
            changeStatus(EStatus.DROP);
        } else {
            shuffle();
        }
    }

    void shuffle() {
        reset();
        changeStatus(EStatus.PREPARE);
    }

    protected override void release() {
        EventSystem2D.Instance.release();
        MoveCardMgr.Instance.OnCommandCursorChange -= onCommandCursorChange;
    }

    void Update() {
        InputMgr.Instance.update();
        checkStatusChange();
    }

    bool checkStatusEndByTime() {
        _statusTime -= Time.deltaTime;

        return _statusTime <= 0;
    }

    void checkStatusChange() {
        if (_nextStatus != _status) {
            if (_status == EStatus.GAME) {
                exitStatusGame();
            }

            _status = _nextStatus;

            _GameMenu._ButtonRestart.Enabled = _status == EStatus.GAME;

            if (_status == EStatus.READY) {
                enterStatusReady();
            } else if (_status == EStatus.PREPARE) {
                enterStatusPrepare();
            } else if (_status == EStatus.DEAL) {
                enterStatusDeal();
            } else if (_status == EStatus.GAME) {
                enterStatusGame();
            } else if (_status == EStatus.DROP) {
                enterStatusDrop();
            }
        }
    }

    void changeStatus(EStatus status) {
        if (status != _status) {
            _nextStatus = status;
        }
    }

    void enterStatusReady() {
        reset();
        enableBtnShuffle();
        _GameBottomMenu._ButtonUndo.Enabled = MoveCardMgr.Instance.CanUndo;

        newRound();
    }

    void enterStatusPrepare() {
        gamePrepare();
        disableBtnShuffle();

        changeStatus(EStatus.DEAL);
    }

    void enterStatusDeal() {
        dealCardToCardDeckes();
    }

    void enterStatusGame() {
        _isRestart = false;
        enableBtnShuffle();
        DeckDrag.Instance.autoMoveCardAndSwitchDeckCardToFinalDeck();

        _roundTimer = Timer.Instance.setInterval(1.0f, () => _GameTopMenu.setTime(++_roundTime));

        _roundTime = 0;
        _GameTopMenu.setTime(_roundTime);
    }

    void exitStatusGame() {
        Timer.Instance.clearTimeOut(_roundTimer);
        _GameTopMenu.setTime(-1);
    }

    int _roundTimer = 0;
    int _roundTime = 0;

    void enterStatusDrop() {
        if (_cards.Count > 0) {
            int numShuffleCardFly = 0;

            foreach (var card in _cards) {
                card.fly(card.transform.position, _SendDeck.transform.position,
                         () => { if (--numShuffleCardFly == 0) shuffle(); },
                         Random.Range(0, 0.2f), 0, false, 20, iTween.EaseType.linear);

                numShuffleCardFly++;
            }
        } else {
            shuffle();
        }
    }

    public EStatus Status { get { return _status; } }

    EStatus _status = EStatus.NONE;
    EStatus _nextStatus = EStatus.NONE;
    float _statusTime = 0;

    void enableBtnShuffle() {
        _GameMenu._ButtonNewRound.Enabled = true;
    }

    void disableBtnShuffle() {
        _GameMenu._ButtonNewRound.Enabled = false;
    }

    void gamePrepare() {
        _GameTopMenu.setTime(-1);
        createCards();
    }

    void reset() {
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

    void createCards() {
        if (!_isRestart) {
            shuffleCards();
        }

        foreach (var id in _startCards) {
            createCard(id);
        }
    }

    void shuffleCards() {
        List<int> cards = new List<int>();

        // generate card deckes by card id
        // 0 ~ 12, spade
        // 13 ~ 25, heard
        // 26 ~ 38, club
        // 39 ~ 51, diamond
        int num = 52;

        for (int i = 0; i < num; i++) {
            cards.Add(i);
        }

        _startCards.Clear();

        while (num > 0) {
            var idx = Random.Range(0, num);

            _startCards.Add(cards[idx]);
            cards[idx] = cards[num - 1];

            num--;
        }
    }

    List<int> _startCards = new List<int>();

    void dealCardToCardDeckes() {
        int[] numCardDecks = new int[8] { 6, 7, 6, 7, 6, 7, 6, 7 };

        if (_isRestart) {
            for (int i = 0; i < _startNumCardDecks.Length; i++) {
                numCardDecks[i] = _startNumCardDecks[i];
            }
        } else {
            for (int i = 0; i < 4; i++) {
                int idx = Random.Range(0, numCardDecks.Length);

                var temp = numCardDecks[idx];
                numCardDecks[idx] = numCardDecks[numCardDecks.Length - 1];
                numCardDecks[numCardDecks.Length - 1] = temp;
            }

            for (int i = 0; i < numCardDecks.Length; i++) {
                _startNumCardDecks[i] = numCardDecks[i];
            }
        }

		int idxCard = 0;
        int numFlyCard = 0;

		for (int i = 0; i < 8 && idxCard < _cards.Count; i++) {
			int numCard = numCardDecks[i];

			while (numCard-- > 0 && idxCard < _cards.Count) {
                _DeckCards[i].putOnCard(_cards[idxCard++]);
                numFlyCard++;
			}
		}

        float delayFlyCard = 0.5f;
        float zOffsetFlyCard = 0;

        _DeckCards.ForEach(deck => {
            if (deck.BottomCard != null) {
                deck.BottomCard.foreachCardUp(card => {
                    card.fly(_SendDeck.transform.position, card.transform.position,
                            () => { if (--numFlyCard == 0) changeStatus(EStatus.GAME); },
                            delayFlyCard, zOffsetFlyCard, true, 0, iTween.EaseType.easeOutExpo);

                    zOffsetFlyCard += -0.1f;
                    delayFlyCard += Config.Instance.DealCardInterval;
                });
            }
        });
    }

    int[] _startNumCardDecks = new int[8];

    Card createCard(int id) {
        var card = GameObject.Instantiate(ResourceMgr.Instance.getCardPrefab(),
                                          _SendDeck.transform, false);

        card.init(id);

        _cards.Add(card);

		return card;
	}

    List<Card> _cards = new List<Card>();

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
