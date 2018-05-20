using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceMgr : MonoSingleton<ResourceMgr> {

	[System.Serializable]
	public class CardData {
		public Color[] _ColorTypes = new Color[4] {
			Color.black, Color.red, Color.black, Color.red
		};
		public Sprite[] _SpriteTypes = new Sprite[4];
		public Sprite[] _SpriteValues = new Sprite[13];
	}

    public CardData _CardData;
    public Card _CardPrefab;

    public Card getCardPrefab() {
        return _CardPrefab;
    }

	public Color getColorCardType(global::CardData.ECardType cardType) {
		return _CardData._ColorTypes[(int)cardType];
	}

	public Sprite getSpriteCardType(global::CardData.ECardType cardType) {
		return _CardData._SpriteTypes[(int)cardType];
	}

	public Sprite getSpriteCardVal(int val) {
		if (val >= 0 && val <= 12) {
			return _CardData._SpriteValues[val];
		}

		return null;
	}

#if UNITY_EDITOR
	const string CARD_NO_SPRITE_SHEET_PATH = "Assets/texture/poker_no.png";
	const string CARD_COLOR_SPRITE_SHEET_PATH = "Assets/texture/poker_color.png";

	[ContextMenu("AutoSetCardResource")]
	void autoSetCardResource() {
		Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(CARD_NO_SPRITE_SHEET_PATH).OfType<Sprite>().ToArray();

		foreach (var sp in sprites) {
			for (int i = 1; i <= 13; i++) {
                if (sp.name == i.ToString()) {
					_CardData._SpriteValues[i - 1] = sp;
				}
			}
        }

		string[] namesCardType = new string[4] {
			"spade", "heart", "club", "diamond"
		};

		sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(CARD_COLOR_SPRITE_SHEET_PATH).OfType<Sprite>().ToArray();

		foreach (var sp in sprites) {
			for (int i = 0; i < 4; i++) {
                if (sp.name == namesCardType[i]) {
					_CardData._SpriteTypes[i] = sp;
					continue;
                }
			}
		}
	}
#endif

}
