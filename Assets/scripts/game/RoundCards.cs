using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCards { 
    public RoundCards() {
        for (int i = 0; i < 52; i++) {
            _cards.Add(i);
        }

        int[] numCardOnDeckes = new int[] { 6, 7, 6, 7, 6, 7, 6, 7 };

        for (int i = 0; i < numCardOnDeckes.Length; i++) {
            _numCardOnDeckes.Add(numCardOnDeckes[i]);
        }
    }

    public void each(System.Action<int, int, int, int> fn) {
        if (fn == null) return;

        int idx = 0;

        for (int i = 0; i < _numCardOnDeckes.Count; i++) {
            int numDeckCards = _numCardOnDeckes[i];

            for (int j = 0; j < numDeckCards; j++) {
                fn(i, j, idx, _cards[idx]);
                idx++;
            }
        }
    }

    public void shuffle() {
        shuffleCards();
        randomNumCardOnDeckes();
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

        _cards.Clear();

        while (num > 0) {
            var idx = Random.Range(0, num);

            _cards.Add(cards[idx]);
            cards[idx] = cards[num - 1];

            num--;
        }
    }

    void randomNumCardOnDeckes() {
        for (int i = 0; i < 4; i++) {
            int idx = Random.Range(0, _numCardOnDeckes.Count);

            var temp = _numCardOnDeckes[idx];
            _numCardOnDeckes[idx] = _numCardOnDeckes[_numCardOnDeckes.Count - 1];
            _numCardOnDeckes[_numCardOnDeckes.Count - 1] = temp;
        }
    }

    List<int> _cards = new List<int>();
    List<int> _numCardOnDeckes = new List<int>();
}
