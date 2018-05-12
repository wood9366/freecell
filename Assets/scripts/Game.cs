using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public List<Deck> _DeckSwitches = new List<Deck>(4);
	public List<DeckFinal> _DeckFinals = new List<DeckFinal>(4);
	public List<DeckCard> _DeckCards = new List<DeckCard>(8);

	static public Game Instance { get { return sInstance; } }
	static Game sInstance = null;

	void Awake() {
		sInstance = this;
	}

	void Start() {
		gameStart();
    }

	void gameStart() {
		foreach (var deck in _DeckSwitches) {
			deck.empty();
		}

		foreach (var deck in _DeckFinals) {
			deck.empty();
		}

		foreach (var deck in _DeckCards) {
			deck.empty();
		}

		// generate card deckes by card id
		// 0 ~ 12, spade
		// 13 ~ 25, heard
		// 26 ~ 38, club
		// 39 ~ 51, diamond
		int num = 52;
		int[] cards = new int[num];

		for (int i = 0; i < num; i++) {
			cards[i] = i;
		}

		for (int i = 0; i < 50; i++) {
			int idx = Random.Range(0, num);

			var temp = cards[idx];
			cards[idx] = cards[num - 1];
			cards[num - 1] = temp;
		}


		// fill cards into card deckes
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
				var id = cards[cur++];

                if (CardData.IsValidCardId(id)) {
					_DeckCards[i].addCard(createCard(id));
				} else {
					Debug.LogWarningFormat("fill card desk {0} with invalid id {1}", i, id);
				}

				if (cur >= num) {
					break;
				}
			}

			if (cur >= num) {
				break;
			}
		}
	}

	Card createCard(int id) {
		var obj = GameObject.Instantiate(ResourceMgr.Instance.getCardPrefab(),
                                         Vector3.zero,
                                         Quaternion.identity);

        obj.init(id);

		return obj;
	}

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
