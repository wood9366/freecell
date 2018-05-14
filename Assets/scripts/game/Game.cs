using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoSingleton<Game> {
    public GameObject _SendDeck = null;
	public List<Deck> _DeckSwitches = new List<Deck>(4);
	public List<DeckFinal> _DeckFinals = new List<DeckFinal>(4);
	public List<DeckCard> _DeckCards = new List<DeckCard>(8);

    public enum EStatus {
        INIT = 0,
        DEAL,
        GAME
    }

    void Update() {
        if (_status == EStatus.INIT) {
            gameInit();

            _status = EStatus.DEAL;
        } else if (_status == EStatus.DEAL) {
            if (_statusTime == 0) {
                float delay = 1.0f;

                foreach (var card in _cards) {
                    iTween.MoveFrom(card.gameObject,
                                    iTween.Hash("delay", delay,
                                                "position", _SendDeck.transform.position,
                                                "speed", Config.Instance.CardFlySpeed));
                    delay += Config.Instance.DealCardInterval;
                }
            }

            _statusTime += Time.deltaTime;

            if (_statusTime >= (_cards.Count - 1) * Config.Instance.DealCardInterval + 2.0f) {
                _statusTime = 0;
                _status = EStatus.GAME;
            }
        }
    }

    public EStatus Status { get { return _status; } }

    EStatus _status = EStatus.INIT;
    float _statusTime = 0;

    void gameInit() {
        reset();
        createCards();
        shuffleCards();
        fillCardDeckes();
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

    void fillCardDeckes() {
		int[] numCardDecks = new int[8] { 6, 7, 6, 7, 6, 7, 6, 7 };

		for (int i = 0; i < 4; i++) {
			int idx = Random.Range(0, numCardDecks.Length);

			var temp = numCardDecks[idx];
			numCardDecks[idx] = numCardDecks[numCardDecks.Length - 1];
			numCardDecks[numCardDecks.Length - 1] = temp;
		}

		int cur = 0;

		for (int i = 0; i < 8; i++) {
			int numCard = numCardDecks[i];

			while (numCard-- > 0) {
                _DeckCards[i].putOnCard(_cards[cur++]);

                if (cur >= _cards.Count) {
					break;
				}
			}

			if (cur >= _cards.Count) {
				break;
			}
		}
    }

    Card createCard(int id) {
		var card = GameObject.Instantiate(ResourceMgr.Instance.getCardPrefab(),
                                         Vector3.zero,
                                         Quaternion.identity);

        card.init(id);

        _cards.Add(card);

		return card;
	}

    List<Card> _cards = new List<Card>();

#if UNITY_EDITOR
	[ContextMenu("Auto Set Deck")]
	void autoSetDeck() {
		setDeck<Deck>("2d/SwitchDecks", _DeckSwitches);
		setDeck<DeckFinal>("2d/FinalDecks", _DeckFinals);
		setDeck<DeckCard>("2d/CardDecks", _DeckCards);
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
