using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData {

	// card id
	// 0 ~ 12, spade
	// 13 ~ 25, heard
	// 26 ~ 38, club
	// 39 ~ 51, diamond
    static public CardData Create(int id) {
        CardData card = null;

		if (id >= 0 && id <= 12) {
			card = Create(ECardType.SPADE, id);
		} else if (id >= 13 && id <= 25) {
			card = Create(ECardType.HEART, id - 13);
		} else if (id >= 26 && id <= 38) {
			card = Create(ECardType.CLUB, id - 26);
		} else if (id >= 39 && id <= 51) {
			card = Create(ECardType.DIAMOND, id - 39);
		}

        return card;
    }
    
    static public CardData Create(ECardType type, int val) {
        CardData card = null;

        if (IsValidCardVal(val)) {
            card = new CardData(type, val);
        }

        return card;
    }

    static public bool IsValidCardId(int id) {
        return id >= 0 && id <= 51;
    }

    static bool IsValidCardVal(int val) {
        return val >= 0 && val <= 12;
    }

	public enum ECardType {
		SPADE = 0,
		HEART,
		CLUB,
		DIAMOND
	}

    public enum ECardTypeColor {
        RED = 0,
        BLACK
    }

	CardData(ECardType type, int val) {
		_type = type;
		_val = val;
	}

	public ECardType Type { get { return _type; } }
	public int Val { get { return _val; } }

    public ECardTypeColor TypeColor {
        get {
            if (Type == ECardType.DIAMOND || Type == ECardType.HEART) {
                return ECardTypeColor.RED;
            } else {
                return ECardTypeColor.BLACK;
            }
        }
    }

	override public string ToString() {
		return "Card " + Type.ToString() + " " + Val.ToString();
	}

	ECardType _type = ECardType.SPADE;
	int _val = 0;
}
