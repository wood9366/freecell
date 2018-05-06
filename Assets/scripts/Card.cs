using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
	static public Card Empty = new Card(ECardType.NONE, 0);

	public enum ECardType {
		SPADE = 0,
		HEART,
		CLUB,
		DIAMOND,

		NUM,

		NONE
	}

	public Card(ECardType type, int val) {
		set(type, val);
	}

	public Card(Card card) {
		set(card);
	}

	// card id
	// 0 ~ 12, spade
	// 13 ~ 25, heard
	// 26 ~ 38, club
	// 39 ~ 51, diamond
	public Card(int id) {
		if (id >= 0 && id <= 12) {
			set(ECardType.SPADE, id + 1);
		} else if (id >= 13 && id <= 25) {
			set(ECardType.HEART, id - 12);
		} else if (id >= 26 && id <= 38) {
			set(ECardType.CLUB, id - 25);
		} else if (id >= 39 && id <= 51) {
			set(ECardType.DIAMOND, id - 38);
		} else {
			set(ECardType.NONE, 0);
		}
	}

	public void set(Card card) {
		set(card._type, card._val);
	}

	public void set(ECardType type, int val) {
		_type = type;
		_val = val;
	}

	public bool IsValid {
		get {
            return _type >= 0 && _type < ECardType.NUM
                && _val >= 1 && _val <= 13;
		}
	}

	public ECardType Type { get { return _type; } }
	public int Val { get { return _val; } }

	override public string ToString() {
		return "Card " + Type.ToString() + " " + Val.ToString();
	}

	ECardType _type = ECardType.NONE;
	int _val = 0;
}