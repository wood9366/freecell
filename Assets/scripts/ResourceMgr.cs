using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceMgr : MonoBehaviour {

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

    static public ResourceMgr Instance { get { return sInstance; } }

	static ResourceMgr sInstance = null;

	void Awake() {
		sInstance = this;
	}

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
		if (val > 0 && val <= 13) {
			return _CardData._SpriteValues[val - 1];
		}

		return null;
	}

#if UNITY_EDITOR
	const string CARD_SPRITE_SHEET_PATH = "Assets/texture/poker.png";

	[ContextMenu("AutoSetCardResource")]
	void autoSetCardResource() {
		Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(CARD_SPRITE_SHEET_PATH).OfType<Sprite>().ToArray();

		string[] namesCardType = new string[4] {
			"spade", "heart", "club", "diamond"
		};

		foreach (var sp in sprites) {
			for (int i = 1; i <= 13; i++) {
                if (sp.name == i.ToString()) {
					_CardData._SpriteValues[i - 1] = sp;
					continue;
				}
			}

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
