using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoSingleton<Game> {
    public GameObject _SendDeck = null;
	public List<Deck> _DeckSwitches = new List<Deck>(4);
	public List<DeckFinal> _DeckFinals = new List<DeckFinal>(4);
	public List<DeckCard> _DeckCards = new List<DeckCard>(8);
    public GameObject _BtnShuffle;
    public Wood9366.Button _BtnUndo;
    public Wood9366.Toggle _ToggleAuto;

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
    }

    bool _isAutoPutCardToFinal = false;

    protected override void init() {
        EventSystem2D.Instance.init();

        if (_BtnShuffle != null) {
            EventListener2D.Get(_BtnShuffle).OnClick = onClickBtnShuffle;
        }

        if (_ToggleAuto != null) {
            _ToggleAuto.IsOn = IsAutoPutCardToFinal;
            _ToggleAuto.OnToggle = onToggleAuto;
        }

        changeStatus(EStatus.READY);
    }

    void onClickBtnShuffle() {
        if (Status == EStatus.GAME) {
            changeStatus(EStatus.DROP);
        } else {
            shuffle();
        }
    }

    void onToggleAuto(bool isOn) {
        _isAutoPutCardToFinal = isOn;

        if (isOn) {
            DeckDrag.Instance.autoMoveCardAndSwitchDeckCardToFinalDeck();
        }
    }

    void shuffle() {
        reset();
        changeStatus(EStatus.PREPARE);
    }

    protected override void release() {
        EventSystem2D.Instance.release();
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
            _status = _nextStatus;

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
        _BtnUndo.IsEnabled = false;
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
        enableBtnShuffle();
        _BtnUndo.IsEnabled = true;
        DeckDrag.Instance.autoMoveCardAndSwitchDeckCardToFinalDeck();
    }

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
        _BtnShuffle.GetComponent<Collider2D>().enabled = true;
    }

    void disableBtnShuffle() {
        _BtnShuffle.GetComponent<Collider2D>().enabled = false;
    }

    void gamePrepare() {
        createCards();
        shuffleCards();
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
    }

    void createCards() {
		// generate card deckes by card id
		// 0 ~ 12, spade
		// 13 ~ 25, heard
		// 26 ~ 38, club
		// 39 ~ 51, diamond
		int num = 52;

		for (int i = 0; i < num; i++) {
            createCard(i);
		}
    }

    void shuffleCards() {
		for (int i = 0; i < 50; i++) {
			int idx = Random.Range(0, _cards.Count);

			var temp = _cards[idx];
			_cards[idx] = _cards[_cards.Count - 1];
			_cards[_cards.Count - 1] = temp;
		}
    }

    void dealCardToCardDeckes() {
		int[] numCardDecks = new int[8] { 6, 7, 6, 7, 6, 7, 6, 7 };

		for (int i = 0; i < 4; i++) {
			int idx = Random.Range(0, numCardDecks.Length);

			var temp = numCardDecks[idx];
			numCardDecks[idx] = numCardDecks[numCardDecks.Length - 1];
			numCardDecks[numCardDecks.Length - 1] = temp;
		}

        float delayFlyCard = 0.5f;
        float zOffsetFlyCard = 0;
        int numFlyCard = 0;
		int idxCard = 0;

		for (int i = 0; i < 8 && idxCard < _cards.Count; i++) {
			int numCard = numCardDecks[i];

			while (numCard-- > 0 && idxCard < _cards.Count) {
                Card card = _cards[idxCard++];

                _DeckCards[i].putOnCard(card);

                card.fly(_SendDeck.transform.position, card.transform.position,
                        () => { if (--numFlyCard == 0) changeStatus(EStatus.GAME); },
                         delayFlyCard, zOffsetFlyCard, true, 0, iTween.EaseType.easeOutExpo);

                numFlyCard++;
                zOffsetFlyCard += -0.1f;
                delayFlyCard += Config.Instance.DealCardInterval;
			}
		}
    }

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
	}

	void setDeck<T>(string path, List<T> _deckes) {
		var obj = GameObject.Find(path);

		if (obj != null) {
			var deckes = obj.GetComponentsInChildren<T>();

			_deckes.Clear();

			foreach (var deck in deckes) {
				_deckes.Add(deck);
			}
		}
	}
#endif
}
